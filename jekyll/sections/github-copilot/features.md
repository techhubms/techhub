---
layout: "page"
title: "Plans & Feature Highlights"
description: "An overview of GitHub Copilot plans and features."
category: "GitHub Copilot"
permalink: "/github-copilot/features.html"
---

This page provides a comprehensive overview of GitHub Copilot plans as of August 29th 2025, combining official features with example videos. For the most current pricing, visit [GitHub's official pricing page](https://github.com/features/copilot/plans).

**Note**: GitHub Copilot is not currently available for GitHub Enterprise Server. For the most recent plan details [view this page](https://docs.github.com/en/copilot/get-started/plans).

{% comment %} Set up global date variable for filtering {% endcomment %}
{% assign now_epoch = '' | now_epoch %}

<div class="features-vertical-container">
  <!-- Subscription Tiers Container -->
  <div class="subscription-wrapper">
    <!-- Free Plan -->
    <div class="subscription-tiers-container full-width">
      <div class="subscription-section">
        <h2>Free</h2>
        <p>A fast way to get started with GitHub Copilot</p>
        <ul>
          <li>50 agent mode or chat requests per month</li>
          <li>2,000 completions per month</li>
          <li>Access to Haiku 4.5, GPT-4.1, and more</li>
        </ul>
        <div class="tier-link">
          <a href="#videos-free">View Features →</a>
        </div>
      </div>
    </div>
    <!-- Pro Plan -->
    <div class="subscription-tiers-container half-width">
      <div class="subscription-section">
        <h2>Pro</h2>
        <p>Accelerate workflows with GitHub Copilot ($10 USD per month or $100 per year)</p>
        <ul>
          <li>Everything in Free</li>
          <li>Coding agent</li>
          <li>Unlimited agent mode and chats with GPT-5 mini</li>
          <li>Unlimited code completions</li>
          <li>Access to code review, Claude Sonnet 4/4.5, GPT-5, Gemini 2.5 Pro, and more</li>
          <li>300 premium requests to use latest models, with option to buy more</li>
          <li>Free for verified students, teachers, and maintainers of popular open source projects</li>
        </ul>
        <div class="tier-link">
          <a href="#videos-pro">View Features →</a>
        </div>
      </div>
    </div>
    <!-- Business Plan -->
    <div class="subscription-tiers-container half-width">
      <div class="subscription-section">
        <h2>Business</h2>
        <p>For teams and organizations ($19 USD per user per month)</p>
        <ul>
          <li>Everything in Pro</li>
          <li>Coding agent for organizations</li>
          <li>Usage metrics and analytics</li>
          <li>Data excluded from training by default</li>
          <li>User management and policies</li>
          <li>Content exclusions</li>
          <li>Audit logs</li>
          <li>300 premium requests per month</li>
        </ul>
        <div class="tier-link">
          <a href="#videos-pro">View Features →</a>
        </div>
      </div>
    </div>
    <!-- Pro+ Plan -->
    <div class="subscription-tiers-container half-width">
      <div class="subscription-section">
        <h2>Pro+</h2>
        <p>Scale with agents and more models ($39 USD per month or $390 per year)</p>
        <ul>
          <li>Everything in Pro</li>
          <li>Access to all models, including Claude Opus 4.1 and more</li>
          <li>1500 Premium Requests per month, with option to buy more</li>
          <li>Access to GitHub Spark</li>
          <li>Codex IDE extension support in VS Code</li>
        </ul>
        <div class="tier-link">
          <a href="#videos-proplus">View Features →</a>
        </div>
      </div>
    </div>
    <!-- Enterprise Plan -->
    <div class="subscription-tiers-container half-width">
      <div class="subscription-section">
        <h2>Enterprise</h2>
        <p>Full enterprise features and controls ($39 USD per user per month)</p>
        <ul>
          <li>Everything in Business</li>
          <li>1000 premium requests per month</li>
          <li>Copilot knowledge bases</li>
          <li>Advanced compliance and security features</li>
          <li>Custom policies and content exclusions</li>
          <li>Enterprise-grade audit logs</li>
          <li>SAML SSO integration</li>
        </ul>
        <div class="tier-link">
          <a href="#videos-proplus">View Features →</a>
        </div>
      </div>
    </div>
  </div>

  <!-- Free Features Section (videos that do not include Pro/Pro+/Business/Enterprise) -->
  {% assign feature_videos = site.videos | where: "section", "github-copilot" | where_exp: "video", "video.plans and video.plans.size > 0" | sort: "title" %}

  <div id="videos-free">
    <h2 class="section-title">Free Features</h2>
  </div>

  <!-- Filter buttons for this section -->
  <div class="ghes-filter section-filter">
    <button class="ghes-toggle-btn">Show features with GHES support</button>
    <button class="video-toggle-btn">Show features with videos</button>
  </div>

  <!-- Features for Free tier -->
  <div class="tier-videos">
    <div class="videos-grid">
      {% for video in feature_videos %}
      {% unless video.plans contains 'Free' %}{% continue %}{% endunless %}
      {% assign video_epoch = video.date | date_to_epoch %}
      {% if video_epoch <= now_epoch %}
      <a href="{{ video.url | relative_url }}" class="video-card-link">
        <div class="video-card" data-ghes="{{ video.ghes_support }}" data-title="{{ video.title }}">
          <div class="video-header">
            <h3>{{ video.title }}</h3>
            <div class="video-icons">
              {% if video.ghes_support == "true" %}
                <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
              {% else %}
                <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
              {% endif %}
              <span class="play-icon" title="View Demo Video">
                <span class="play-triangle"></span>
              </span>
            </div>
          </div>
          <div class="feature-description">
            <p>{{ video.description }}</p>
          </div>
        </div>
      </a>
      {% else %}
      <div class="video-card" data-ghes="{{ video.ghes_support }}" data-title="{{ video.title }}">
        <div class="video-header">
          <h3>{{ video.title }}</h3>
          <div class="video-icons">
            {% if video.ghes_support == "true" %}
              <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
            {% else %}
              <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
            {% endif %}
          </div>
        </div>
        <div class="feature-description">
          <p>{{ video.description }}</p>
        </div>
      </div>
      {% endif %}
      {% endfor %}
    </div>
  </div>

  <!-- Pro & Business Features Section (videos that include Pro or Business but not Pro+ or Enterprise) -->
  <div id="videos-pro">
    <h2 class="section-title">Pro & Business Features</h2>
  </div>

  <!-- Filter buttons for this section -->
  <div class="ghes-filter section-filter">
    <button class="ghes-toggle-btn">Show features with GHES support</button>
    <button class="video-toggle-btn">Show features with videos</button>
  </div>

  <!-- Features for Pro & Business tiers -->
  <div class="tier-videos">
    <div class="videos-grid">
      {% for video in feature_videos %}
      {% unless video.plans contains 'Pro' or video.plans contains 'Business' %}{% continue %}{% endunless %}
      {% assign video_epoch = video.date | date_to_epoch %}
      {% if video_epoch <= now_epoch %}
      <a href="{{ video.url | relative_url }}" class="video-card-link">
        <div class="video-card" data-ghes="{{ video.ghes_support }}" data-title="{{ video.title }}">
          <div class="video-header">
            <h3>{{ video.title }}</h3>
            <div class="video-icons">
              {% if video.ghes_support == "true" %}
                <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
              {% else %}
                <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
              {% endif %}
              <span class="play-icon" title="View Demo Video">
                <span class="play-triangle"></span>
              </span>
            </div>
          </div>
          <div class="feature-description">
            <p>{{ video.description }}</p>
          </div>
        </div>
      </a>
      {% else %}
      <div class="video-card" data-ghes="{{ video.ghes_support }}" data-title="{{ video.title }}">
        <div class="video-header">
          <h3>{{ video.title }}</h3>
          <div class="video-icons">
            {% if video.ghes_support == "true" %}
              <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
            {% else %}
              <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
            {% endif %}
          </div>
        </div>
        <div class="feature-description">
          <p>{{ video.description }}</p>
        </div>
      </div>
      {% endif %}
      {% endfor %}
    </div>
  </div>

  <!-- Pro+ & Enterprise Features Section (videos that include Pro+ or Enterprise) -->
  <div id="videos-proplus">
    <h2 class="section-title">Pro+ & Enterprise Features</h2>
  </div>

  <!-- Filter buttons for this section -->
  <div class="ghes-filter section-filter">
    <button class="ghes-toggle-btn">Show features with GHES support</button>
    <button class="video-toggle-btn">Show features with videos</button>
  </div>

  <!-- Features for Pro+ & Enterprise tiers -->
  <div class="tier-videos">
    <div class="videos-grid">
      {% for video in feature_videos %}
      {% unless video.plans contains 'Pro+' or video.plans contains 'Enterprise' %}{% continue %}{% endunless %}
      {% assign video_epoch = video.date | date_to_epoch %}
      {% if video_epoch <= now_epoch %}
      <a href="{{ video.url | relative_url }}" class="video-card-link">
        <div class="video-card" data-ghes="{{ video.ghes_support }}" data-title="{{ video.title }}">
          <div class="video-header">
            <h3>{{ video.title }}</h3>
            <div class="video-icons">
              {% if video.ghes_support == "true" %}
                <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
              {% else %}
                <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
              {% endif %}
              <span class="play-icon" title="View Demo Video">
                <span class="play-triangle"></span>
              </span>
            </div>
          </div>
          <div class="feature-description">
            <p>{{ video.description }}</p>
          </div>
        </div>
      </a>
      {% else %}
      <div class="video-card" data-ghes="{{ video.ghes_support }}" data-title="{{ video.title }}">
        <div class="video-header">
          <h3>{{ video.title }}</h3>
          <div class="video-icons">
            {% if video.ghes_support == "true" %}
              <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
              {% else %}
              <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
            {% endif %}
          </div>
        </div>
        <div class="feature-description">
          <p>{{ video.description }}</p>
        </div>
      </div>
      {% endif %}
      {% endfor %}
    </div>
  </div>

</div><script src="{{ '/assets/js/features.js' | relative_url }}"></script>
