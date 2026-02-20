---
external_url: https://techcommunity.microsoft.com/t5/azure-high-performance-computing/centralized-cluster-performance-metrics-with-reframe-hpc-and/ba-p/4488077
title: Centralizing Cluster Performance Metrics with ReFrame HPC and Azure Log Analytics
author: jimpaine
primary_section: dotnet
feed_name: Microsoft Tech Community
date: 2026-02-06 09:37:24 +00:00
tags:
- Automated Testing
- Azure
- Azure Log Analytics
- Azure Monitor
- Bicep
- Cluster Monitoring
- Community
- Continuous Monitoring
- Data Collection Rule
- DevOps
- HPC
- IaC
- Log Analytics Workspace
- PBS
- Performance Metrics
- Performance Testing
- Python
- ReFrame HPC
- Slurm
- Testing Framework
- .NET
section_names:
- azure
- dotnet
- devops
---
jimpaine demonstrates how to use ReFrame HPC in conjunction with Azure Log Analytics to centralize and analyze performance metrics from multiple HPC clusters, providing actionable insight for system administrators and developers.<!--excerpt_end-->

# Centralized Cluster Performance Metrics with ReFrame HPC and Azure Log Analytics

## Introduction

Managing multiple HPC clusters across dev, test, and production environments—or migrating between different workload schedulers such as PBS and Slurm—can present visibility and performance assurance challenges. jimpaine shows how combining ReFrame HPC, an extensible testing framework, with Azure Log Analytics unlocks powerful unified monitoring and analysis.

## Deploying Azure Resources

Set up necessary Azure infrastructure with [Bicep templates](https://github.com/JimPaine/reframe-azure-perflog-handler):

- **Data Collection Endpoint**: Endpoint receiving logs from external systems
- **Data Collection Rule**: Defines how incoming data is processed
- **Log Analytics Workspace**: Central log and metrics storage for analysis

> ⚠️ Capture the endpoint URL output by the Bicep deployment—it will be required for ReFrame logging configuration.

## Running an IOR Benchmark Test with ReFrame HPC

Example Python-based ReFrame test that runs IOR for both read and write benchmarks:

```python
import reframe as rfm
import reframe.utility.sanity as sn

@rfm.simple_test
class SimplePerfTest(rfm.RunOnlyRegressionTest):
    valid_systems = ["*"]
    valid_prog_environs = ["+ior"]
    executable = 'ior'
    executable_opts = ['-a POSIX -w -r -C -e -g -F -b 2M -t 2M -s 25600 -o /data/demo/test.bin -D 300']
    reference = {
        'tst:hbv4': {
            'write_bandwidth_mib': (500, -0.05, 0.1, 'MiB/s'),
            'read_bandwidth_mib': (350, -0.05, 0.5, 'MiB/s'),
        }
    }

    @sanity_function
    def validate_run(self):
        return sn.assert_found(r'Summary of all tests:', self.stdout)

    @performance_function('MiB/s')
    def write_bandwidth_mib(self):
        return sn.extractsingle(r'^write\s+([0-9]+\.?[0-9]*)', self.stdout, 1, float)

    @performance_function('MiB/s')
    def read_bandwidth_mib(self):
        return sn.extractsingle(r'^read\s+([0-9]+\.?[0-9]*)', self.stdout, 1, float)
```

- **`executable`**: IOR binary with workload options
- **`valid_systems`**: Any cluster with IOR available
- **Sanity & performance functions**: Extract test results and check expected output

Learn more about [system/environment configuration in ReFrame](https://reframe-hpc.readthedocs.io/en/stable/tutorial.html#systems-and-environments).

## Configuring ReFrame for Azure Log Analytics Integration

A core part of unifying metrics is configuring the logging handler in ReFrame's config. Key elements:

- `perflog_multiline`: Set to `True` for single-record metrics per log line
- **Logging handler example**:

  ```python
  'logging': [{
      'perflog_multiline': True,
      'handlers_perflog': [{
          'type': 'httpjson',
          'url': '<your endpoint>',
          'level': 'info',
          'debug': False,
          'extra_headers': {'Authorization': f'Bearer {_get_token()}'},
          'extras': {
              'TimeGenerated': f'{datetime.now(timezone.utc).isoformat()}',
              'facility': 'reframe',
              'reframe_azure_data_version': '1.0',
          },
          'ignore_keys': ['check_perfvalues'],
          'json_formatter': _format_record
      }]
  }]
  ```

- **Bearer token**: Use a Python function to acquire an Azure AD token for authentication:

  ```python
  def _get_token(scope='https://monitor.azure.com/.default') -> str:
      credential = DefaultAzureCredential()
      token = credential.get_token(scope)
      return token.token
  ```

- **JSON formatting**: Convert records to arrays as required by data ingestion schema

Find full example configs [on GitHub](https://github.com/JimPaine/reframe-azure-perflog-handler/blob/main/config.py).

## Running Tests and Viewing Results

1. **Authenticate with Azure:**

   ```bash
   az login --identity
   ```

2. **Set up Python environment:**

   ```bash
   python3 -m venv .venv
   source .venv/bin/activate
   pip install -U pip
   pip install -r requirements.txt
   ```

3. **Run ReFrame CLI:**

   ```bash
   reframe -C config.py -c simple_perf.py --performance-report -r
   ```

4. **Check results in Log Analytics:**
   - Use KQL queries to analyze trends, benchmarks, and cluster performance metrics across your environments.

## Benefits of Unified Monitoring

- **Fast cross-cluster comparisons**
- **Long-term trend analysis**
- **Standardized metrics and schema**
- **Unified log search and reporting**

This workflow provides ongoing confidence for migrations, system maintenance, and identifying bottlenecks or regressions.

## Conclusion

Standardizing on ReFrame HPC for workload validation—and exporting results to Azure Log Analytics—empowers administrators and engineers with actionable observability, regardless of platform or cluster management software.

For more, see the [ReFrame HPC documentation](https://reframe-hpc.readthedocs.io/en/stable/index.html) and [Jim Paine's Azure integration example repo](https://github.com/JimPaine/reframe-azure-perflog-handler).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-high-performance-computing/centralized-cluster-performance-metrics-with-reframe-hpc-and/ba-p/4488077)
