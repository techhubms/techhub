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
      <h1 class="section-title">
        {% for plan in entry.plans %}{{ plan.name }}{% unless forloop.last %} & {% endunless %}{% endfor %}
      </h1>
      <hr />
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

<style>
/* Main container with vertical stacking */
.features-vertical-container {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

/* Wrapper for subscription containers */
.subscription-wrapper {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
  margin-bottom: 10px;
}

/* GHES Filter Button */
.ghes-filter {
  text-align: left;
}

.ghes-toggle-btn, .video-toggle-btn {
  border: none;
  border-radius: 6px;
  padding: 10px 15px;
  cursor: pointer;
  font-size: 14px;
  font-weight: 500;
  transition: background-color 0.2s;
  margin-right: 10px;
  margin-bottom: 10px;
  color: white;
}

.ghes-toggle-btn {
  background-color: #28a745;
}

.video-toggle-btn {
  background-color: #0366d6;
}

.ghes-toggle-btn.active,
.video-toggle-btn.active {
  background-color: #6c757d;
}

/* Hover effects only on devices that support hover */
@media (hover: hover) and (pointer: fine) {
  .ghes-toggle-btn:hover {
    background-color: #218838;
  }

  .video-toggle-btn:hover {
    background-color: #0258c5;
  }
}

/* Subscription tiers container for horizontal layout */
.subscription-tiers-container {
  display: flex;
  flex-wrap: wrap;
  gap: 20px;
}

/* Full width entries (100%) - plans stack vertically */
.subscription-tiers-container.full-width {
  flex-direction: column;
  width: 100%;
}

/* Half width entries (50%) - plans display horizontally */
.subscription-tiers-container.half-width {
  flex-direction: row;
  width: calc(50% - 10px); /* Account for gap */
}

/* Subscription section styling */
.subscription-section {
  background: rgba(31, 111, 235, 0.10);
  border: 1px solid #bd93f9;
  border-radius: 12px;
  padding: 5px 15px 15px 15px;
  box-shadow: 0 8px 25px rgba(31, 111, 235, 0.18);
  flex: 1;
  min-width: 250px; /* Minimum width before wrapping */
  transition: all 0.2s ease;
}

/* Tier link styling */
.tier-link {
  margin-top: 15px;
  text-align: left;
  font-size: 20px;
}

.tier-link a {
  color: #58a6ff !important;
}

.tier-link a:visited {
  color: #58a6ff !important;
}

/* Hover effects only on devices that support hover */
@media (hover: hover) and (pointer: fine) {
  .tier-link a:hover {
    color: #79c0ff !important;
  }
}

/* Videos section styling */
.videos-section-title {
  border-bottom: 1px solid #e1e4e8;
}

/* Video grid layouts */
.videos-grid {
  display: grid;
  gap: 10px;
  margin-bottom: 25px;
  grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  align-items: stretch;
}

/* Video card styling */
.video-card-link {
  text-decoration: none !important;
  color: inherit;
  display: flex;
  height: 100%;
}

.video-card {
  background: rgba(31, 111, 235, 0.10);
  border: 1px solid #bd93f9;
  border-radius: 12px;
  padding: 15px;
  box-shadow: 0 8px 25px rgba(31, 111, 235, 0.18);
  transition: all 0.2s ease;
  display: flex;
  flex-direction: column;
}

.video-card-link:hover {
  text-decoration: none !important;
}

/* Hover effects only on devices that support hover */
@media (hover: hover) and (pointer: fine) {
  .video-card-link:hover .video-card {
    transform: scale(1.05);
    background: rgba(31, 111, 235, 0.18);
  }
}

/* Feature description styling */
.feature-description {
  flex: 1;
  margin: 10px 0;
}

.feature-description p {
  font-size: 13px;
  line-height: 1.4;
  margin: 0;
  color: #c9d1d9;
}

/* Video header with title and icons */
.video-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 8px;
}

.video-card h3 {
  font-size: 14px;
  font-weight: 500;
  margin: 0;
  line-height: 1.3;
  flex: 1;
}

/* Video icons container */
.video-icons {
  display: flex;
  gap: 6px;
  align-items: center;
  flex-shrink: 0;
}

/* Play button icon */
.play-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 20px;
  height: 20px;
  background-color: #0366d6;
  border-radius: 50%;
  transition: background-color 0.2s;
}

.play-triangle {
  width: 0;
  height: 0;
  border-left: 8px solid white;
  border-top: 5px solid transparent;
  border-bottom: 5px solid transparent;
  margin-left: 2px; /* Slight offset to center the triangle visually */
}

/* GHES support icon */
.ghes-icon {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 18px;
  height: 18px;
  color: white;
  border-radius: 50%;
  font-size: 12px;
  font-weight: bold;
}

.ghes-supported {
  background-color: #28a745;
}

.ghes-not-supported {
  background-color: #dc3545;
  display: none; /* Hide by default */
}

/* Entry separator styling */
.entry-separator {
  margin: 0px 0px 25px 0px;
  border: 0;
  height: 1px;
  background-color: rgb(255, 255, 255, 0.5);
}

/* Responsive adjustments */
/* Mobile: < 768px */
@media (max-width: 767px) {
  .videos-grid {
    grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
    gap: 8px;
  }

  .video-card {
    padding: 10px;
  }
  
  .video-card h3 {
    font-size: 13px;
  }
  
  .subscription-section {
    flex: 0 0 100%; /* Full width on small screens */
    min-width: 0; /* Override minimum width to prevent overflow */
  }

  .subscription-wrapper {
    flex-direction: column;
  }

  .subscription-tiers-container.half-width {
    width: 100%; /* Full width on mobile */
    flex-direction: column; /* Stack plans vertically on mobile */
  }

  .subscription-tiers-container {
    flex-wrap: nowrap; /* Prevent wrapping on small screens */
  }
}

/* Tablet: 768px - 1024px */
@media (min-width: 768px) and (max-width: 1024px) {
  .videos-grid {
    grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
    gap: 9px;
  }

  .subscription-section {
    min-width: 280px;
  }
}

/* Desktop: > 1024px */
@media (min-width: 1025px) {
  .videos-grid {
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
  }
}
</style>

<!-- JavaScript for filter toggle buttons -->
<script>
document.addEventListener('DOMContentLoaded', function() {
  const filterSections = document.querySelectorAll('.section-filter');

  filterSections.forEach(function(section) {
    const ghesToggleBtn = section.querySelector('.ghes-toggle-btn');
    const videoToggleBtn = section.querySelector('.video-toggle-btn');

    // Error handling: ensure buttons exist
    if (!ghesToggleBtn || !videoToggleBtn) {
      return;
    }

    let ghesOnly = false;
    let videoOnly = false;

    // Find the next videos-grid after this section
    const nextVideosGrid = section.nextElementSibling?.querySelector('.videos-grid');
    if (!nextVideosGrid) {
      return;
    }

    const videoCards = nextVideosGrid.querySelectorAll('.video-card, .video-card-link');
    if (!videoCards.length) {
      return;
    }

    function applyFilters() {
      videoCards.forEach(card => {
        const videoCard = card.classList.contains('video-card') ? card : card.querySelector('.video-card');
        if (!videoCard) {
          return;
        }

        const ghesSupport = videoCard.getAttribute('data-ghes') === 'true';
        const hasVideo = videoCard.querySelector('.play-icon') !== null;

        let shouldShow = true;

        if (ghesOnly && !ghesSupport) {
          shouldShow = false;
        }

        if (videoOnly && !hasVideo) {
          shouldShow = false;
        }

        card.style.display = shouldShow ? '' : 'none';
      });
    }

    ghesToggleBtn.addEventListener('click', function() {
      ghesOnly = !ghesOnly;
      ghesToggleBtn.classList.toggle('active');

      if (ghesOnly) {
        ghesToggleBtn.textContent = 'Show all features';
      } else {
        ghesToggleBtn.textContent = 'Show features with GHES support';
      }

      applyFilters();
    });

    videoToggleBtn.addEventListener('click', function() {
      videoOnly = !videoOnly;
      videoToggleBtn.classList.toggle('active');

      if (videoOnly) {
        videoToggleBtn.textContent = 'Show all features';
      } else {
        videoToggleBtn.textContent = 'Show features with videos';
      }

      applyFilters();
    });
  });
});
</script>
