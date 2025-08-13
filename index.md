---
# **Critical for AI**: This is not documentation, but the home page of the Tech Hub.

# Feel free to add content and custom Front Matter to this file.
# To modify the layout, see https://jekyllrb.com/docs/themes/#overriding-theme-defaults
layout: "home"
---

# Tech Hub

Welcome! Because we know it's hard to stay up-to-date with the fast changing world of IT, here is a website where you'll find blogs, news, videos and more content about technology in the Microsoft space. Constantly updated and pre-screened to only provide useful content.

<h2>Last 4 Roundups</h2>
<div class="site-roundups">
  <ul>
    {%- assign sorted_roundups = site.roundups | sort: 'date' | reverse -%}
    {% for roundup in sorted_roundups limit:4 %}
      <li>
        <a class="roundup-title" href="{{ roundup.url | relative_url }}">{{ roundup.title | escape }}</a>
      </li>
    {% endfor %}
  </ul>
</div>

<h2>Content per category</h2>
<div class="sections-grid">
    {% for section in site.data.sections %}
      {% assign section_key = section[0] %}
      {% assign section_data = section[1] %}
      <a href="{{ section_data.url | relative_url }}" class="section-square" style="background-image: url('{{ section_data.image | relative_url }}');">
        <div class="section-content">
          <span class="section-title">{{ section_data.title }}</span>
          <span class="section-desc">{{ section_data.description }}</span>
        </div>
      </a>
    {% endfor %}
</div>
