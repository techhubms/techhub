---
layout: "page"
title: "AI Native SDLC"
description: "A comprehensive guide to the Software Development Life Cycle with AI enhancements at every phase."
category: "AI"
page-name: "sdlc"
---

<script type="module">
    import mermaid from 'https://cdn.jsdelivr.net/npm/mermaid@11/dist/mermaid.esm.min.mjs';
    mermaid.initialize({ 
        startOnLoad: true,
        theme: 'dark',
        securityLevel: 'loose',
        flowchart: {
            htmlLabels: true,
            curve: 'basis'
        }
    });
</script>

<div class="sdlc-container">
    <div class="sdlc-intro">
        <p>The Software Development Life Cycle (SDLC) is a structured framework that guides teams through creating high-quality software efficiently. Each phase builds upon the previous, with clear handovers ensuring smooth transitions. AI enhances every phase—from analyzing requirements to predicting system failures.</p>
    </div>
    
    <div class="sdlc-legend">
        <div class="sdlc-legend-items">
            <div class="sdlc-legend-item">
                <div class="sdlc-legend-box sdlc-legend-step"></div>
                <span>Process Steps</span>
            </div>
            <div class="sdlc-legend-item">
                <div class="sdlc-legend-box sdlc-legend-handover"></div>
                <span>Handovers</span>
            </div>
        </div>
        <button class="sdlc-expand-all-btn" onclick="sdlcToggleAll()">Expand/Collapse All</button>
    </div>

    <div class="sdlc-diagram-section">
        <div class="sdlc-mermaid-container">
            <div class="sdlc-mermaid">
                <div class="mermaid">
                    graph TD
                        Planning["📋 Planning"]
                        Design["🎨 Design"]
                        Implementation["⚙️ Implementation"]
                        Testing["🧪 Testing"]
                        Deployment["🚀 Deployment"]
                        Maintenance["🔧 Maintenance"]
                        
                        Planning -->|" "| Design
                        Design -->|" "| Implementation
                        Implementation -->|" "| Testing
                        Testing -->|" "| Deployment
                        Deployment -->|" "| Maintenance
                        Maintenance -.->|Feedback| Planning
                        
                        click Planning sdlcOpenCardPlanning
                        click Design sdlcOpenCardDesign
                        click Implementation sdlcOpenCardImplementation
                        click Testing sdlcOpenCardTesting
                        click Deployment sdlcOpenCardDeployment
                        click Maintenance sdlcOpenCardMaintenance
                        
                        linkStyle 0 stroke:#cccccc,stroke-width:3px
                        linkStyle 1 stroke:#cccccc,stroke-width:3px
                        linkStyle 2 stroke:#cccccc,stroke-width:3px
                        linkStyle 3 stroke:#cccccc,stroke-width:3px
                        linkStyle 4 stroke:#cccccc,stroke-width:3px
                        
                        style Planning fill:#e3f2fd,stroke:#1976d2,stroke-width:3px,cursor:pointer
                        style Design fill:#f3e5f5,stroke:#7b1fa2,stroke-width:3px,cursor:pointer
                        style Implementation fill:#e8f5e9,stroke:#388e3c,stroke-width:3px,cursor:pointer
                        style Testing fill:#fff3e0,stroke:#f57c00,stroke-width:3px,cursor:pointer
                        style Deployment fill:#fce4ec,stroke:#c2185b,stroke-width:3px,cursor:pointer
                        style Maintenance fill:#e0f2f1,stroke:#00796b,stroke-width:3px,cursor:pointer
                </div>
            </div>
        </div>

        <div class="sdlc-info-panel">
            <!-- Planning Step -->
            <div class="sdlc-step-card" id="sdlc-card-planning">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>📋 Planning</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">What</div>
                        <div class="sdlc-info-text">Gather and analyze requirements from stakeholders, define project scope, establish timelines, and create a comprehensive roadmap. This phase determines the project's technical, operational, and economic feasibility.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">How</div>
                        <div class="sdlc-info-text">Conduct stakeholder interviews, gather functional and non-functional requirements, perform feasibility analysis, define acceptance criteria, and create user stories with clear definitions of done.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">AI Enhancement</div>
                        <div class="sdlc-info-text">AI analyzes customer feedback and user data to extract actionable insights, helps identify requirement patterns, suggests acceptance criteria based on similar projects, and assists in writing user stories.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Agile Practices</div>
                        <div class="sdlc-info-text">Organize requirements into a prioritized product backlog, break work into sprint-sized increments, and use iterative planning to adapt to changing needs.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Azure DevOps Boards</span>
                            <span class="sdlc-tool-tag">GitHub Projects</span>
                            <span class="sdlc-tool-tag">Microsoft Planner</span>
                            <span class="sdlc-tool-tag">GitHub Copilot</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Handover 1 -->
            <div class="sdlc-handover-card" id="sdlc-card-handover1">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>Planning → Design</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Deliverables</div>
                        <div class="sdlc-info-text">Approved requirements document, user stories with acceptance criteria, prioritized product backlog, technical constraints, and compliance requirements.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Process</div>
                        <div class="sdlc-info-text">Conduct requirements review meeting with design team, obtain stakeholder sign-off, and ensure all questions are documented and answered before design begins.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">GitHub Wiki</span>
                            <span class="sdlc-tool-tag">SharePoint</span>
                            <span class="sdlc-tool-tag">Microsoft Teams</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Design Step -->
            <div class="sdlc-step-card" id="sdlc-card-design">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>🎨 Design</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">What</div>
                        <div class="sdlc-info-text">Create system architecture, define data models, design user interfaces, and establish technical specifications that translate requirements into a detailed blueprint for development.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">How</div>
                        <div class="sdlc-info-text">Develop high-level and detailed architecture diagrams, create wireframes and interactive prototypes, define API contracts, establish coding standards, and conduct design reviews with stakeholders.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">AI Enhancement</div>
                        <div class="sdlc-info-text">AI generates architecture diagrams, suggests design patterns based on requirements, creates code scaffolding from specifications, and helps identify potential scalability or security concerns early.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Best Practices</div>
                        <div class="sdlc-info-text">Design for modularity and reusability, consider security requirements from the start, plan for testability, and document architectural decisions and their rationale.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Figma</span>
                            <span class="sdlc-tool-tag">Azure Architecture Center</span>
                            <span class="sdlc-tool-tag">Microsoft Visio</span>
                            <span class="sdlc-tool-tag">GitHub Copilot</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Handover 2 -->
            <div class="sdlc-handover-card" id="sdlc-card-handover2">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>Design → Implementation</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Deliverables</div>
                        <div class="sdlc-info-text">System architecture diagrams, API specifications, database schemas, UI mockups, coding standards, and development environment setup instructions.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Process</div>
                        <div class="sdlc-info-text">Design handoff meeting with development team, walkthrough of architecture decisions, establish version control branching strategy, and set up initial repository structure.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Figma Dev Mode</span>
                            <span class="sdlc-tool-tag">GitHub Issues</span>
                            <span class="sdlc-tool-tag">Azure DevOps Work Items</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Implementation Step -->
            <div class="sdlc-step-card" id="sdlc-card-implementation">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>⚙️ Implementation</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">What</div>
                        <div class="sdlc-info-text">Write, review, and integrate code to build the software according to design specifications. This phase transforms the blueprint into a functional product.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">How</div>
                        <div class="sdlc-info-text">Develop in iterative sprints, use feature branches and pull requests, conduct code reviews, maintain continuous integration pipelines, and follow coding standards.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">AI Enhancement</div>
                        <div class="sdlc-info-text">AI provides real-time code suggestions and autocompletion, generates boilerplate code, assists with debugging, explains complex code segments, and helps refactor for better performance.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Version Control Best Practices</div>
                        <div class="sdlc-info-text">Write clear, descriptive commit messages. Test code before committing. Use branches for features and fixes. Review changes before merging. Pull changes frequently to stay current.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Code Review Process</div>
                        <div class="sdlc-info-text">Prepare and understand changes before review. Request reviews via pull requests. Provide constructive feedback on logic, security, and maintainability. Discuss disagreements collaboratively. Approve and merge when ready.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Visual Studio Code</span>
                            <span class="sdlc-tool-tag">Visual Studio</span>
                            <span class="sdlc-tool-tag">GitHub Copilot</span>
                            <span class="sdlc-tool-tag">GitHub</span>
                            <span class="sdlc-tool-tag">Git</span>
                            <span class="sdlc-tool-tag">.NET</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Handover 3 -->
            <div class="sdlc-handover-card" id="sdlc-card-handover3">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>Implementation → Testing</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Deliverables</div>
                        <div class="sdlc-info-text">Completed code with unit tests passing, test environment deployment, test cases mapped to requirements, and known issues documentation.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Process</div>
                        <div class="sdlc-info-text">Feature demonstration to QA team, test environment verification, review test plan coverage, and establish defect tracking workflow.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Azure Test Plans</span>
                            <span class="sdlc-tool-tag">GitHub Pull Requests</span>
                            <span class="sdlc-tool-tag">Azure DevOps</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Testing Step -->
            <div class="sdlc-step-card" id="sdlc-card-testing">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>🧪 Testing</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">What</div>
                        <div class="sdlc-info-text">Verify functionality, identify defects, validate security, and ensure the software meets quality standards and user requirements before release.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Testing Types</div>
                        <div class="sdlc-info-text"><strong>Unit Testing:</strong> Test individual components in isolation. <strong>Integration Testing:</strong> Verify components work together. <strong>Functional Testing:</strong> Validate against requirements. <strong>Regression Testing:</strong> Ensure changes don't break existing features. <strong>User Acceptance Testing:</strong> End-users validate the system. <strong>Security Testing:</strong> Identify vulnerabilities. <strong>Performance Testing:</strong> Validate under load conditions.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">AI Enhancement</div>
                        <div class="sdlc-info-text">AI auto-generates test cases from code and requirements, predicts high-risk areas for focused testing, identifies patterns in bug reports, and suggests test coverage improvements.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Best Practices</div>
                        <div class="sdlc-info-text">Start testing early in the development cycle. Write comprehensive test cases covering edge cases. Automate repetitive tests. Prioritize security testing. Document and track all defects. Re-test after fixes.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">MSTest / xUnit</span>
                            <span class="sdlc-tool-tag">Playwright</span>
                            <span class="sdlc-tool-tag">Azure Load Testing</span>
                            <span class="sdlc-tool-tag">GitHub Advanced Security</span>
                            <span class="sdlc-tool-tag">GitHub Actions</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Handover 4 -->
            <div class="sdlc-handover-card" id="sdlc-card-handover4">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>Testing → Deployment</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Deliverables</div>
                        <div class="sdlc-info-text">Test reports with all critical tests passing, security scan results, performance test validation, UAT sign-off, and release notes.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Process</div>
                        <div class="sdlc-info-text">Go/no-go decision meeting, final stakeholder approval, deployment checklist verification, and rollback plan confirmation.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">GitHub Actions</span>
                            <span class="sdlc-tool-tag">Azure Pipelines</span>
                            <span class="sdlc-tool-tag">Azure DevOps Release</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Deployment Step -->
            <div class="sdlc-step-card" id="sdlc-card-deployment">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>🚀 Deployment</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">What</div>
                        <div class="sdlc-info-text">Release the software to production environments, configure infrastructure, and make the application available to end users with minimal disruption.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">How</div>
                        <div class="sdlc-info-text">Use automated CI/CD pipelines, implement blue-green or canary deployment strategies, maintain rollback procedures, and monitor deployment health in real-time.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">AI Enhancement</div>
                        <div class="sdlc-info-text">AI predicts optimal deployment timing, monitors rollout health, detects anomalies during deployment, and suggests rollback triggers based on historical patterns.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">DevOps Integration</div>
                        <div class="sdlc-info-text">Continuous Delivery ensures code is always in a deployable state. Continuous Deployment automates releases to production. Infrastructure as Code manages environments consistently. Feature flags enable gradual rollouts.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Azure App Service</span>
                            <span class="sdlc-tool-tag">Azure Kubernetes Service</span>
                            <span class="sdlc-tool-tag">GitHub Actions</span>
                            <span class="sdlc-tool-tag">Azure Bicep</span>
                            <span class="sdlc-tool-tag">Azure DevOps Pipelines</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Handover 5 -->
            <div class="sdlc-handover-card" id="sdlc-card-handover5">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>Deployment → Maintenance</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Deliverables</div>
                        <div class="sdlc-info-text">Operations runbook, monitoring dashboards configured, on-call escalation procedures, and support documentation with troubleshooting guides.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Process</div>
                        <div class="sdlc-info-text">Knowledge transfer sessions with support team, handover of administrative access, alert threshold configuration, and incident response drill.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Azure DevOps Wiki</span>
                            <span class="sdlc-tool-tag">GitHub Discussions</span>
                            <span class="sdlc-tool-tag">Microsoft Teams</span>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Maintenance Step -->
            <div class="sdlc-step-card" id="sdlc-card-maintenance">
                <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
                    <span>🔧 Maintenance</span>
                    <span class="sdlc-card-icon">▼</span>
                </div>
                <div class="sdlc-card-content">
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">What</div>
                        <div class="sdlc-info-text">Monitor system health, fix bugs, apply security patches, optimize performance, and gather user feedback to drive continuous improvement and future iterations.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">How</div>
                        <div class="sdlc-info-text">Implement proactive monitoring and alerting, establish incident response procedures, analyze user feedback systematically, and maintain documentation for operational knowledge.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">AI Enhancement</div>
                        <div class="sdlc-info-text">AI detects system anomalies before they become incidents, predicts potential failures based on patterns, analyzes logs for root cause identification, and helps prioritize issues by impact.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Continuous Feedback Loop</div>
                        <div class="sdlc-info-text">User feedback and operational insights flow back to the Planning phase, enabling iterative improvements. This creates a cycle where each release informs the next development iteration.</div>
                    </div>
                    <div class="sdlc-info-section">
                        <div class="sdlc-info-label">Tools</div>
                        <div class="sdlc-tools-list">
                            <span class="sdlc-tool-tag">Azure Monitor</span>
                            <span class="sdlc-tool-tag">Application Insights</span>
                            <span class="sdlc-tool-tag">GitHub Issues</span>
                            <span class="sdlc-tool-tag">Azure DevOps Boards</span>
                            <span class="sdlc-tool-tag">Microsoft Teams</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Additional Information Section -->
    <div class="sdlc-additional-info-section">
        <h2>Additional Information</h2>
    </div>

    <!-- Benefits Section -->
    <div class="sdlc-section-card" id="sdlc-card-benefits">
        <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
            <span>✓ Benefits of a Structured SDLC</span>
            <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content">
            <div class="sdlc-benefits-grid">
                <div class="sdlc-benefit-card">
                    <span class="sdlc-benefit-icon">✓</span>
                    <div>
                        <strong>Improved Quality</strong>
                        <p>Systematic testing and reviews catch defects early, reducing bugs in production.</p>
                    </div>
                </div>
                <div class="sdlc-benefit-card">
                    <span class="sdlc-benefit-icon">✓</span>
                    <div>
                        <strong>Clear Communication</strong>
                        <p>Defined phases and handovers ensure all stakeholders stay aligned throughout development.</p>
                    </div>
                </div>
                <div class="sdlc-benefit-card">
                    <span class="sdlc-benefit-icon">✓</span>
                    <div>
                        <strong>Predictable Delivery</strong>
                        <p>Structured planning and tracking enable accurate timelines and resource allocation.</p>
                    </div>
                </div>
                <div class="sdlc-benefit-card">
                    <span class="sdlc-benefit-icon">✓</span>
                    <div>
                        <strong>Reduced Risk</strong>
                        <p>Early requirement validation and iterative feedback minimize costly late-stage changes.</p>
                    </div>
                </div>
                <div class="sdlc-benefit-card">
                    <span class="sdlc-benefit-icon">✓</span>
                    <div>
                        <strong>Security Integration</strong>
                        <p>Security considerations are embedded at each phase rather than added as an afterthought.</p>
                    </div>
                </div>
                <div class="sdlc-benefit-card">
                    <span class="sdlc-benefit-icon">✓</span>
                    <div>
                        <strong>Continuous Improvement</strong>
                        <p>Feedback loops from maintenance inform future iterations, creating a learning organization.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Challenges Section -->
    <div class="sdlc-section-card" id="sdlc-card-challenges">
        <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
            <span>⚠️ Common Challenges</span>
            <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content">
            <div class="sdlc-challenges-grid">
                <div class="sdlc-challenge-item">
                    <strong>Scope Creep</strong>
                    <p>Requirements grow beyond original scope. Mitigate with clear change management processes and backlog prioritization.</p>
                </div>
                <div class="sdlc-challenge-item">
                    <strong>Communication Gaps</strong>
                    <p>Information lost between phases. Address with clear documentation, shared tools, and regular cross-team meetings.</p>
                </div>
                <div class="sdlc-challenge-item">
                    <strong>Technical Debt</strong>
                    <p>Shortcuts accumulate over time. Plan regular refactoring cycles and maintain coding standards.</p>
                </div>
                <div class="sdlc-challenge-item">
                    <strong>Testing Bottlenecks</strong>
                    <p>Testing becomes a blocker late in the cycle. Shift-left by integrating testing earlier and automating where possible.</p>
                </div>
            </div>
        </div>
    </div>

    <!-- Methodologies Section -->
    <div class="sdlc-section-card" id="sdlc-card-methodologies">
        <div class="sdlc-card-header" onclick="sdlcToggleCard(this)">
            <span>📋 SDLC and Development Methodologies</span>
            <span class="sdlc-card-icon">▼</span>
        </div>
        <div class="sdlc-card-content">
            <p class="sdlc-methodologies-intro">The SDLC phases shown above define <strong>what work needs to happen</strong>. Development methodologies define <strong>how that work is organized and executed</strong>. Every methodology uses these same phases—the difference is in timing, iteration, and flow.</p>
            
            <div class="sdlc-methodology-detail">
                <div class="sdlc-methodology-header">
                    <strong>Waterfall (Traditional)</strong>
                    <span class="sdlc-methodology-tag sequential">Sequential</span>
                </div>
                <p>Each SDLC phase completes fully before the next begins. All requirements are gathered upfront, design is finalized before coding, and testing happens only after implementation. Best for projects with well-defined, stable requirements.</p>
                <div class="sdlc-methodology-flow">
                    <span>Planning</span><span class="sdlc-flow-arrow">→</span>
                    <span>Design</span><span class="sdlc-flow-arrow">→</span>
                    <span>Implementation</span><span class="sdlc-flow-arrow">→</span>
                    <span>Testing</span><span class="sdlc-flow-arrow">→</span>
                    <span>Deployment</span>
                </div>
            </div>

            <div class="sdlc-methodology-detail">
                <div class="sdlc-methodology-header">
                    <strong>Agile / Scrum</strong>
                    <span class="sdlc-methodology-tag iterative">Iterative</span>
                </div>
                <p>All SDLC phases happen within each sprint (typically 2-4 weeks). A small slice of requirements is planned, designed, built, tested, and potentially deployed in each iteration. Feedback from each sprint informs the next, enabling rapid adaptation to changing requirements.</p>
                <div class="sdlc-methodology-flow">
                    <div class="sdlc-sprint-box">
                        <span class="sdlc-sprint-label">Sprint 1</span>
                        <span>Plan → Design → Build → Test → Deploy</span>
                    </div>
                    <span class="sdlc-flow-arrow">→</span>
                    <div class="sdlc-sprint-box">
                        <span class="sdlc-sprint-label">Sprint 2</span>
                        <span>Plan → Design → Build → Test → Deploy</span>
                    </div>
                    <span class="sdlc-flow-arrow">→</span>
                    <span>...</span>
                </div>
            </div>

            <div class="sdlc-methodology-detail">
                <div class="sdlc-methodology-header">
                    <strong>Kanban</strong>
                    <span class="sdlc-methodology-tag continuous">Continuous Flow</span>
                </div>
                <p>Work items flow continuously through SDLC phases without fixed iterations. Work-in-progress limits prevent bottlenecks at any phase. Items move from Planning through Deployment as capacity allows, with no batch releases—each feature ships when ready.</p>
                <div class="sdlc-methodology-flow sdlc-kanban-flow">
                    <div class="sdlc-kanban-column">Planning<br/><small>■ ■</small></div>
                    <div class="sdlc-kanban-column">Design<br/><small>■</small></div>
                    <div class="sdlc-kanban-column">Implementation<br/><small>■ ■ ■</small></div>
                    <div class="sdlc-kanban-column">Testing<br/><small>■ ■</small></div>
                    <div class="sdlc-kanban-column">Deployment<br/><small>■</small></div>
                </div>
            </div>

            <div class="sdlc-methodology-detail">
                <div class="sdlc-methodology-header">
                    <strong>DevOps / CI/CD</strong>
                    <span class="sdlc-methodology-tag automated">Automated</span>
                </div>
                <p>DevOps automates the handovers between SDLC phases, especially from Implementation through Deployment. Continuous Integration automatically tests code on every commit. Continuous Deployment automates releases to production. Monitoring in Maintenance feeds insights back to Planning, closing the loop.</p>
                <div class="sdlc-methodology-flow">
                    <span>Planning</span><span class="sdlc-flow-arrow">→</span>
                    <span>Design</span><span class="sdlc-flow-arrow">→</span>
                    <div class="sdlc-cicd-box">
                        <span class="sdlc-cicd-label">CI/CD Pipeline</span>
                        <span>Implementation → Testing → Deployment</span>
                    </div>
                    <span class="sdlc-flow-arrow">→</span>
                    <span>Maintenance</span>
                    <span class="sdlc-flow-arrow feedback">↺</span>
                </div>
            </div>

            <div class="sdlc-methodology-comparison">
                <h3>Choosing a Methodology</h3>
                <table>
                    <thead>
                        <tr>
                            <th>Methodology</th>
                            <th>Best For</th>
                            <th>SDLC Cycle Time</th>
                            <th>Change Flexibility</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Waterfall</td>
                            <td>Stable requirements, regulated industries</td>
                            <td>Months to years</td>
                            <td>Low</td>
                        </tr>
                        <tr>
                            <td>Agile/Scrum</td>
                            <td>Evolving requirements, customer collaboration</td>
                            <td>2-4 weeks per sprint</td>
                            <td>High</td>
                        </tr>
                        <tr>
                            <td>Kanban</td>
                            <td>Continuous delivery, support/maintenance teams</td>
                            <td>Continuous</td>
                            <td>Very High</td>
                        </tr>
                        <tr>
                            <td>DevOps</td>
                            <td>Frequent releases, automation-ready teams</td>
                            <td>Hours to days</td>
                            <td>High</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</div>

<script>
    function sdlcToggleCard(header) {
        const content = header.nextElementSibling;
        const icon = header.querySelector('.sdlc-card-icon');
        
        content.classList.toggle('expanded');
        icon.classList.toggle('expanded');
    }

    function sdlcToggleAll() {
        const allContents = document.querySelectorAll('.sdlc-card-content');
        const allIcons = document.querySelectorAll('.sdlc-card-icon');
        const isAnyExpanded = Array.from(allContents).some(content => content.classList.contains('expanded'));
        
        if (isAnyExpanded) {
            allContents.forEach(content => content.classList.remove('expanded'));
            allIcons.forEach(icon => icon.classList.remove('expanded'));
        } else {
            allContents.forEach(content => content.classList.add('expanded'));
            allIcons.forEach(icon => icon.classList.add('expanded'));
        }
    }

    function sdlcOpenCard(cardId) {
        const card = document.getElementById('sdlc-card-' + cardId);
        if (card) {
            const content = card.querySelector('.sdlc-card-content');
            const icon = card.querySelector('.sdlc-card-icon');
            
            content.classList.toggle('expanded');
            icon.classList.toggle('expanded');
            
            if (content.classList.contains('expanded')) {
                card.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
                card.style.boxShadow = '0 0 20px rgba(88, 166, 255, 0.5)';
                setTimeout(() => {
                    card.style.boxShadow = '';
                }, 2000);
            }
        }
    }

    // Functions for Mermaid diagram clicks
    window.sdlcOpenCardPlanning = function() { sdlcOpenCard('planning'); };
    window.sdlcOpenCardDesign = function() { sdlcOpenCard('design'); };
    window.sdlcOpenCardImplementation = function() { sdlcOpenCard('implementation'); };
    window.sdlcOpenCardTesting = function() { sdlcOpenCard('testing'); };
    window.sdlcOpenCardDeployment = function() { sdlcOpenCard('deployment'); };
    window.sdlcOpenCardMaintenance = function() { sdlcOpenCard('maintenance'); };
    
    // Functions for handover cards
    window.sdlcOpenCardHandover1 = function() { sdlcOpenCard('handover1'); };
    window.sdlcOpenCardHandover2 = function() { sdlcOpenCard('handover2'); };
    window.sdlcOpenCardHandover3 = function() { sdlcOpenCard('handover3'); };
    window.sdlcOpenCardHandover4 = function() { sdlcOpenCard('handover4'); };
    window.sdlcOpenCardHandover5 = function() { sdlcOpenCard('handover5'); };

    // Make handover lines clickable after Mermaid renders
    window.addEventListener('load', function() {
        setTimeout(function() {
            const svg = document.querySelector('.sdlc-mermaid svg');
            if (svg) {
                const allPaths = svg.querySelectorAll('path.flowchart-link');
                const handoverHandlers = [
                    sdlcOpenCardHandover1,
                    sdlcOpenCardHandover2,
                    sdlcOpenCardHandover3,
                    sdlcOpenCardHandover4,
                    sdlcOpenCardHandover5
                ];
                
                let handoverIndex = 0;
                
                allPaths.forEach((path) => {
                    // Skip if we already have all 5 handovers
                    if (handoverIndex >= handoverHandlers.length) {
                        return;
                    }
                    
                    // Skip dashed lines (feedback line)
                    const strokeDasharray = path.getAttribute('stroke-dasharray');
                    if (strokeDasharray && strokeDasharray !== 'none' && strokeDasharray !== '0') {
                        return;
                    }
                    
                    const stroke = path.getAttribute('stroke');
                    const style = path.getAttribute('style');

                    if ((stroke && stroke.includes('#cccccc')) || 
                        (style && (style.includes('#cccccc') || style.includes('rgb(204, 204, 204)'))) ||
                        (path.style.stroke && path.style.stroke.includes('#cccccc'))) {
                        
                        const parent = path.closest('g.edgePath') || path.parentElement;
                        const handler = handoverHandlers[handoverIndex];
                        
                        const clickHandler = function(e) {
                            e.stopPropagation();
                            e.preventDefault();
                            handler();
                        };
                        
                        path.style.cursor = 'pointer';
                        path.style.pointerEvents = 'stroke';
                        path.addEventListener('click', clickHandler);
                        
                        if (parent) {
                            parent.style.cursor = 'pointer';
                            parent.style.pointerEvents = 'all';
                            parent.addEventListener('click', clickHandler);
                        }
                        
                        handoverIndex++;
                    }
                });
            }
        }, 1000);
    });
</script>
