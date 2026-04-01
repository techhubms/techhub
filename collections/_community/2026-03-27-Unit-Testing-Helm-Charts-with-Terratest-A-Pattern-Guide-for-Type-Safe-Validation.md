---
title: 'Unit Testing Helm Charts with Terratest: A Pattern Guide for Type-Safe Validation'
feed_name: Microsoft Tech Community
primary_section: devops
date: 2026-03-27 07:33:36 +00:00
section_names:
- devops
tags:
- AppArmor
- Apps/v1 Deployment
- Azure DevOps Pipelines
- CI/CD
- Community
- ConfigMap
- DevOps
- Environment Specific Values
- Go
- Gotestsum
- Helm
- Helm Charts
- Helm Lint
- Helm Template
- HPA
- Ingress
- JUnit
- Kubernetes
- Kubernetes Manifests
- Render And Assert
- Seccomp
- SecurityContext
- Terratest
- Testify
- Unit Testing
- Values.yaml
external_url: https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unit-testing-helm-charts-with-terratest-a-pattern-guide-for-type/ba-p/4506165
author: pratikpanda
---

pratikpanda explains a “render-and-assert” approach to unit testing Helm charts with Terratest and Go, focusing on validating rendered Kubernetes manifests across multiple environments and wiring the tests into Azure DevOps pipelines.<!--excerpt_end-->

## Overview

Helm charts are powerful, but their templating logic (conditionals, loops, environment overrides) can create subtle bugs that slip past `helm lint` and manual review. This guide proposes a **render-and-assert** unit testing approach using **Terratest + Go** to validate the exact Kubernetes manifests your chart produces—**without needing a cluster**.

## The problem Helm chart teams run into

Helm templates generate YAML, and small changes can break behavior across environments even when the output remains syntactically valid.

Common examples:

- `values-prod.yaml` points to the wrong container registry
- A `securityContext` gets removed during a refactor
- Ingress hosts differ between dev and prod due to incorrect overrides
- HPA bounds get swapped across environments
- Label selectors drift, causing orphaned ReplicaSets

`helm lint` and code review typically don’t validate whether the rendered output matches your expectations—only that it’s “valid enough” YAML.

## The approach: Render and assert

Instead of deploying to a cluster, you:

1. **Render**: call `helm template` with `values.yaml` plus an environment override (`values-<env>.yaml`)
2. **Unmarshal**: deserialize YAML into real Kubernetes API structs (for example `appsV1.Deployment`, `coreV1.ConfigMap`, `networkingV1.Ingress`)
3. **Assert**: verify specific fields with `testify` assertions (labels, selectors, security posture, probes, resource limits, ingress routing, etc.)

Key properties:

- No cluster required
- No mocks
- Fast, deterministic tests
- **Type safety**: assertions against Go structs fail at compile time if you reference non-existent fields

### Example (render → unmarshal → assert)

```go
// Arrange
options := &helm.Options{ ValuesFiles: s.valuesFiles }
output := helm.RenderTemplate(s.T(), options, s.chartPath, s.releaseName, s.templates)

// Act
var deployment appsV1.Deployment
helm.UnmarshalK8SYaml(s.T(), output, &deployment)

// Assert: security context is hardened
secCtx := deployment.Spec.Template.Spec.Containers[0].SecurityContext
require.Equal(s.T(), int64(1000), *secCtx.RunAsUser)
require.True(s.T(), *secCtx.RunAsNonRoot)
require.True(s.T(), *secCtx.ReadOnlyRootFilesystem)
require.False(s.T(), *secCtx.AllowPrivilegeEscalation)
```

## What to test: 16 patterns across 6 categories

The post groups practical assertions into six buckets:

| Category | What gets validated |
| --- | --- |
| Identity & Labels | Resource names, standard Helm/K8s labels, selector alignment |
| Configuration | Environment-specific ConfigMap data, env var injection |
| Container | Image registry per env, ports, resource requests/limits |
| Security | Non-root user, read-only FS, dropped capabilities, AppArmor, seccomp, SA token automount |
| Reliability | Startup/liveness/readiness probes, volume mounts |
| Networking & Scaling | Ingress hosts/TLS per env, service port wiring, HPA bounds per env |

Suggested starting point: validate **resource names and labels** first (often catches `_helpers.tpl` mistakes), then add security and environment-specific patterns.

## Multi-environment testing structure

To prevent “works in dev, fails in prod” drift, keep separate test suites per environment, each validating the merged values:

- `values.yaml` + `values-dev.yaml`
- `values.yaml` + `values-test.yaml`
- `values.yaml` + `values-prod.yaml`

Example structure:

```text
tests/unit/my-chart/
├── dev/   # values.yaml + values-dev.yaml
├── test/  # values.yaml + values-test.yaml
└── prod/  # values.yaml + values-prod.yaml
```

## Security as code (CI gate)

The post recommends asserting security posture directly in unit tests so regressions are caught immediately:

- Run as non-root (UID 1000)
- Read-only root filesystem
- Drop Linux capabilities
- Block privilege escalation
- AppArmor profile `runtime/default`
- Seccomp `RuntimeDefault`
- Disable service account token automount

## CI/CD integration with Azure DevOps

Because these are standard Go tests that shell out to Helm under the hood, you can run them in **Azure DevOps Pipelines** with just:

- Helm CLI
- Go runtime

A typical pipeline includes stages like:

```yaml
stages:
- stage: Build
- stage: Dev
- stage: Test
- stage: Production
```

A reusable test template example includes installing `gotestsum`, running Go tests, and publishing JUnit results:

```yaml
- script: |
    export PATH=$PATH:/usr/local/go/bin:$(go env GOPATH)/bin
    go install gotest.tools/gotestsum@latest
    cd $(Pipeline.Workspace)/helm.artifact/tests/unit
    gotestsum --format testname --junitfile $(Agent.TempDirectory)/test-results.xml \
      -- ./${{ parameters.helmTestPath }}/... -count=1 -timeout 50m
  displayName: 'Test helm chart'
  env:
    HELM_RELEASE_NAME: ${{ parameters.helmReleaseName }}
    HELM_VALUES_FILE_OVERRIDE: ${{ parameters.helmValuesFileOverride }}

- task: PublishTestResults@2
  displayName: 'Publish test results'
  inputs:
    testResultsFormat: 'JUnit'
    testResultsFiles: '$(Agent.TempDirectory)/test-results.xml'
  condition: always()
```

## Terratest + Go vs helm-unittest

The post compares Terratest and `helm-unittest`:

- **Type safety**: Go structs + compiler checks vs YAML path matching (typos can silently match nothing)
- **Language features**: loops, shared setup, table-driven tests vs YAML DSL
- **Debugging**: standard Go debugging vs diffs only
- **Ecosystem alignment**: fits teams already using Go/Terraform testing

It notes `helm-unittest` can still be a reasonable choice if the team wants the lowest barrier and doesn’t use Go.

## Getting started checklist

### Suggested repo layout

```text
your-repo/
├── charts/
│   └── your-chart/
│       ├── Chart.yaml
│       ├── values.yaml
│       ├── values-dev.yaml
│       ├── values-test.yaml
│       ├── values-prod.yaml
│       └── templates/
├── tests/
│   └── unit/
│       ├── go.mod
│       └── your-chart/
│           ├── dev/
│           ├── test/
│           └── prod/
└── Makefile
```

### Prerequisites

- Go 1.22+
- Helm 3.14+

### Dependencies called out

- `github.com/gruntwork-io/terratest v0.46.16`
- `github.com/stretchr/testify v1.8.4`
- `k8s.io/api v0.28.4`

### Example local run

```bash
cd tests/unit
HELM_RELEASE_NAME=your-chart \
HELM_VALUES_FILE_OVERRIDE=values-dev.yaml \
go test -v ./your-chart/dev/... -timeout 30m
```

Recommended first test: start with a **ConfigMap** (simpler than Deployments) to validate the render → unmarshal → assert flow.


[Read the entire article](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/unit-testing-helm-charts-with-terratest-a-pattern-guide-for-type/ba-p/4506165)

