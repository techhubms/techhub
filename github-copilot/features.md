---
layout: "page"
title: "Feature Highlights"
description: "An overview of GitHub Copilot plans and features."
category: "GitHub Copilot"
---

This page provides a comprehensive overview of GitHub Copilot plans as of June the 27th 2025, combining official features with example videos.

<div class="features-vertical-container">

  <!-- Subscription Tiers Container -->
  <div class="subscription-wrapper">
    {% for entry in site.data.copilot_plans %}
    <div class="subscription-tiers-container {% if entry.width == '100%' %}full-width{% else %}half-width{% endif %}">
        {% for plan in entry.plans %}
        <div class="subscription-section">
          <h2>{{ plan.name }}</h2>
          <p>{{ plan.description }}</p>
          <ul>
            {% for benefit in plan.benefits %}
            <li>{{ benefit }}</li>
            {% endfor %}
          </ul>
          <div class="tier-link">
            <a href="#videos-{% for plan in entry.plans %}{{ plan.name | downcase | replace: ' ', '-' | replace: '+', 'plus' }}{% unless forloop.last %}-{% endunless %}{% endfor %}">View Features →</a>
          </div>
        </div>
        {% endfor %}
    </div>
    {% endfor %}
  </div>

  {% for entry in site.data.copilot_plans %}
    <div id="videos-{% for plan in entry.plans %}{{ plan.name | downcase | replace: ' ', '-' | replace: '+', 'plus' }}{% unless forloop.last %}-{% endunless %}{% endfor %}">
      <h2 class="section-title">
        {% for plan in entry.plans %}{{ plan.name }}{% unless forloop.last %} & {% endunless %}{% endfor %}
      </h2>
    </div>

    <!-- Filter buttons for this section -->
    <div class="ghes-filter section-filter">
      <button class="ghes-toggle-btn">Show features with GHES support</button>
      <button class="video-toggle-btn">Show features with videos</button>
    </div>

    <!-- Features for this subscription tier -->
    <div class="tier-videos">
      <div class="videos-grid">
        {% for feature in entry.features %}
        {% if feature.videoUrl != "" %}
        <a href="{{ feature.videoUrl }}" class="video-card-link">
          <div class="video-card" data-ghes="{{ feature.ghes_support }}" data-title="{{ feature.title }}">
            <div class="video-header">
              <h3>{{ feature.title }}</h3>
              <div class="video-icons">
                {% if feature.ghes_support %}
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
              <p>{{ feature.description }}</p>
            </div>
          </div>
        </a>
        {% else %}
        <div class="video-card" data-ghes="{{ feature.ghes_support }}" data-title="{{ feature.title }}">
          <div class="video-header">
            <h3>{{ feature.title }}</h3>
            <div class="video-icons">
              {% if feature.ghes_support %}
                <span class="ghes-icon ghes-supported" title="GHES Supported">✓</span>
              {% else %}
                <span class="ghes-icon ghes-not-supported" title="Not GHES Supported">✕</span>
              {% endif %}
            </div>
          </div>
          <div class="feature-description">
            <p>{{ feature.description }}</p>
          </div>
        </div>
        {% endif %}
        {% endfor %}
      </div>
    </div>
  {% endfor %}

</div>

<script src="{{ '/assets/js/features.js' | relative_url }}"></script>
