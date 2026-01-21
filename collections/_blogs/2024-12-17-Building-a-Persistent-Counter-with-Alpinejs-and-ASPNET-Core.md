---
external_url: https://khalidabuhakmeh.com/building-a-persistent-counter-with-alpinejs
title: Building a Persistent Counter with Alpine.js and ASP.NET Core
author: Khalid Abuhakmeh
feed_name: Khalid Abuhakmeh's Blog
date: 2024-12-17 00:00:00 +00:00
tags:
- Alpine.js
- ASP.NET Core
- Blazor
- Client Side
- Counter Example
- DOM Manipulation
- Front End
- HTML
- Htmx
- JavaScript
- Local Storage
- Persistence
- State Management
- Store Pattern
- Web Development
section_names:
- coding
---
Khalid Abuhakmeh explains how to build a persistent counter with Alpine.js, demonstrating concepts similar to Blazor's counter sample and showcasing ASP.NET Core integration.<!--excerpt_end-->

# Building a Persistent Counter with Alpine.js and ASP.NET Core

**By Khalid Abuhakmeh**

![Building a Persistent Counter with Alpine.Js](https://res.cloudinary.com/abuhakmeh/image/fetch/c_limit,f_auto,q_auto,w_800/https://khalidabuhakmeh.com/assets/images/posts/misc/building-persistent-counter-alpinejs-javascript.jpg)

If you work with .NET technologies and modern web development, you may know about the default Blazor "Counter" example. This blog post reimagines that example using [Alpine.js](https://alpinejs.dev/), a declarative JavaScript library for client-side interactivity. The walkthrough demonstrates how to build a self-contained, persistent counter entirely on the client side, highlighting approaches to state management, persistence with local storage, and integration points with ASP.NET Core layouts.

## Installing Alpine.js in an ASP.NET Core Project

To use Alpine.js in your ASP.NET Core application, add a script reference to your layout file:

```html
<!-- Alpine Core -->
<script defer src="https://cdn.jsdelivr.net/npm/alpinejs@3.x.x/dist/cdn.min.js"></script>
```

Optionally, Alpine.js can be installed via NPM and bundled into your build process.

## Building a Simple Inline Counter

Alpine.js uses the `x-data` attribute to provide state to DOM elements. Here's an inline counter example:

```html
<main class="container" x-data="{ count: 0 }">
  <article class="card">
    <header>
      <h3>Counter Value</h3>
    </header>
    <section>
      <p x-text="count"></p>
    </section>
  </article>
  <button type="button" x-on:click="count++">Increment</button>
  <button type="button" x-on:click="count = 0">Reset</button>
</main>
```

## Persisting the Counter State

To retain the counter value across page reloads, Alpine.js offers a persistence plugin. Add it via another script tag:

```html
<!-- Alpine Plugins -->
<script defer src="https://cdn.jsdelivr.net/npm/@alpinejs/persist@3.x.x/dist/cdn.min.js"></script>
```

Then, modify your x-data to use Alpine's `$persist` utility:

```html
<main class="container" x-data="{ count: $persist(0) }">
  <article class="card">
    <header>
      <h3>Counter Value</h3>
    </header>
    <section>
      <p x-text="count"></p>
    </section>
  </article>
  <button type="button" x-on:click="count++">Increment</button>
  <button type="button" x-on:click="count = 0">Reset</button>
</main>
```

Refreshing will now keep the last counter value.

## Using Alpine.js Stores for Global State

If you want to share state between page elements, Alpine.js provides stores. You can create a global, persistent counter like this:

```html
<body>
  <main class="container" x-data>
    <article class="card">
      <header>
        <h3>Counter Value</h3>
      </header>
      <section>
        <p x-text="$store.counter.value"></p>
      </section>
    </article>
    <button type="button" x-on:click="$store.counter.increment()">Increment</button>
    <button type="button" x-on:click="$store.counter.reset()">Reset</button>
  </main>
  <script>
    document.addEventListener('alpine:initializing', () => {
      Alpine.store('counter', {
        value: Alpine.$persist(0),
        increment() { this.value++ },
        reset() { this.value = 0; }
      })
    })
  </script>
</body>
```

## Complete Example

A full HTML file for reference:

```html
<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <title>PicoCSS Boilerplate</title>
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@picocss/pico@2/css/pico.min.css" />
  <link rel="stylesheet" href="/css/site.css"/>
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <meta name="color-scheme" content="light dark"/>
  <!-- Alpine Plugins -->
  <script defer src="https://cdn.jsdelivr.net/npm/@alpinejs/persist@3.x.x/dist/cdn.min.js"></script>
  <!-- Alpine Core -->
  <script defer src="https://cdn.jsdelivr.net/npm/alpinejs@3.x.x/dist/cdn.min.js"></script>
</head>
<body>
  <main class="container" x-data>
    <article class="card">
      <header>
        <h3>Counter Value</h3>
      </header>
      <section>
        <p x-text="$store.counter.value"></p>
      </section>
    </article>
    <button type="button" x-on:click="$store.counter.increment()">Increment</button>
    <button type="button" x-on:click="$store.counter.reset()">Reset</button>
  </main>
  <script>
    document.addEventListener('alpine:initializing', () => {
      Alpine.store('counter', {
        value: Alpine.$persist(0),
        increment() { this.value++ },
        reset() { this.value = 0; }
      })
    })
  </script>
</body>
</html>
```

## Conclusion

Alpine.js is a flexible option for building interactive client-side features, even within ASP.NET Core or .NET contexts. By combining libraries like Alpine.js, Htmx, and Blazor concepts, developers can deliver richer experiences with less code and straightforward state management.

---

**About the Author**  
Khalid Abuhakmeh is a developer advocate at JetBrains focused on .NET technologies and tooling.

This post appeared first on "Khalid Abuhakmeh's Blog". [Read the entire article here](https://khalidabuhakmeh.com/building-a-persistent-counter-with-alpinejs)
