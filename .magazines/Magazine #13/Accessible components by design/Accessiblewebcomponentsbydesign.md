**Accessible** **web components by design**

Websites and apps in the public sector and governments must comply to
the W3C accessibility standards. Everyone should be able to easily find,
view and use these websites. For all other websites there are no
regulations. Research have shown that 97.4% of the top one million
websites don't offer full accessibility. These websites had an average
of 50 high/critical accessibility findings on their homepage [^1]. This
is a lot and most of the time these issues are not hard to resolve.

15% of all people have some sort of disability. For visual impaired
people it's important that a website has good colour contrasts. People
with motor/mobility issues need a website that can be accessed using
just a keyboard or custom input devices. For people with
learning/cognitive problems a website should be structured, intuitive,
calming. People with hearing difficulties would want to have captions on
video. Incidentally anyone could be disabled: Sleep deprivation or
breaking an arm.

Improving accessibility is not just about making a website available for
these 15% of people with a disability. It benefits all people in any
environment. For example, having the correct colour contrasts also helps
people with perfect eyesight using a website, for example, outdoors in
bright sunlight. Having captions on video helps people watch a video on
mute in a busy environment. In general, websites that have full
accessibility are perceived better. More-over Gartner has researched
that Digital products in full Web Content Accessibility Guidelines
(WCAG) Level 2 compliance are expected to outperform their market
competitors by 50% by 2023. (Gartner, 2020 [^2]).

Many big companies have their own identity and branding (design system).
This design system is in many cases expressed through reusable style
classes and accessibility tags which are put on HTML elements. For
different pages, many layouts and patterns are copy-pasted. Websites
grow fast. It's hard to maintain accessibility and over time
accessibility becomes inconsistent, simply forgotten, or not important
anymore.

It\'s important to think about accessibility from the start in your web
development process. In this article I will explain about reusable web
components that are accessible by default. You will see how to implement
a reusable web component using the W3C accessibility standards and how
to test such component on its accessibility features.

After, I\'ll cover the \"by design\" part of the article title and shine
some light on how you could improve the usability of your website.
Usability is about designing products to be effective, efficient, and
satisfying (a.k.a. user experience design). Not to be confused with
accessibility.

Tip: Use the Funkify -- Disability Simulator (Free) Chrome extension
[^3] to simulate some of the common disabilities.**\
**

**Accessible by default component library**

This component library contains all the building blocks from the design
system your UX designer invented. If you create components that are
accessible by default, then you only must implement accessibility once
per type of component. Jacob\'s Law from Laws of UX [^4] (See Laws of UX
paragraph) explains that users spend most of their time on other
websites, so they prefer your website to work the same. This also counts
for the accessibility of your components. Luckily for all types of
components there are well documented patterns about how to make them
accessible based on the W3C standard [^5].

Always apply the dumb vs smart components best practice when creating
components in your component library. Most component should be dumb and
only have little state and presentational logic. Smart components have
more responsibility like doing http calls, managing state. These
components usually consist of many dumb components (building blocks) and
is mapping state onto them. Keep the dumb components small with single
responsibility.

Many people with disabilities make use of screen readers in order to
interact with a website. Screen readers will read out to the user what
is currently visible, focussed, possible options, etc. aria-tags are
used on HTML elements in order to let your screen reader interact with
them. In the next chapter you will see how these aria-tags are used in
order to create an accessible by default checkbox component

Tip: Install the WAVE chrome browser plugin [^6] to easily see if 'aria'
tags are being used (correctly), so screen readers interact with your
website better.

Tip: Enable a screen reader, close your eyes, and try to complete a task
on your website. 😉

## Accessible by default checkbox component

For this example, I'll cover the checkbox component. In many cases you
must implement your own checkboxes, because the regular HTML checkboxes
are very hard to style.

The following is the W3C accessibility standard explanation for a
checkbox:

-   An element has role \`[checkbox]{.underline}\`, so that screen
    readers understand that it's interacting with a checkbox.

-   The checkbox has an accessible label provided by the visible text
    content contained within the element with role checkbox. So, people
    using a screen reader know what checkbox they are interacting with.

-   When (un)checked, the checkbox element has state
    '[aria-checked']{.underline} set to true/false, so that screen
    readers understand that a checkbox is checked/unchecked.

Create a \`dumb\` component in your framework of choice that is used
with the following HTML signature:

HTML

\<custom-checkbox value=\"false\"
changed=\"onChanged(\$event)\"\>Unchecked\</custom-checkbox\>

The implementation of this component is as follows. As you see, it
contains everything the W3C standard explained.

HTML:

\<div class=\"custom-checkbox-style\" role=\"checkbox\"
aria-checked=\"{{ value }}\" tabindex=\"0\"\>

\<slot\>\</slot\>

\</div\>

-   Use tabindex="0" to make the div focusable and interactable.

-   \<slot\>\</slot\> is used for content projection. For the above
    example, the text \`Unchecked\` will be projected here.

Now you never have to think about accessibility for your checkbox
component ever again!

**Accessibility driven test (Testing Library)**

You created the accessible by default component and now you want to test
the accessibility features of it. Testing Library [^7] is a great tool
that can help with this. A web component's unit test should not test the
component's instance and its methods/properties. What is visible to the
user and what the user can interact with is what matters.

Bad example without Testing Library (Angular):

Typescript:

const checkbox: HTMLInputElement =
fixture.nativeElement.querySelector(\'input\');

expect(checkbox.checked).toBeFalsy();

const event = new MouseEvent(\'click\');

checkbox.dispatchEvent(event);

-   Doesn\'t test if this div has the role checkbox

-   Doesn\'t test if the checkbox is labelled for screen readers to
    understand

-   Doesn\'t test if the checkbox has aria-checked

-   Doesn\'t actually click a button

-   It\'s very technical

Example with Testing Library with a user pressing spacebar to check the
checkbox:

it(\'should check checkbox when user presses the spacebar'\', () =\> {

// Mount checkbox component

// Arrange

const checkbox = screen.getByRole(\'checkbox\', { checked: false, name:
\'Unchecked\' });

checkbox.focus();

// Act

userEvent.keyboard(\'{space}\');

// Assert

expect(checkbox).toBeChecked();

// expect value to be emitted

});

-   Tests that a checkbox has role=\"checkbox\"

-   Tests that the checkbox has aria-checked=\"false\"

-   Tests that the checkbox that is labelled for screen readers to pick
    up

-   Tests that the checkbox can receive focus (tabindex="0")

-   It tests user interaction through spacebar keyboard press, following
    the W3C standard.

You should add additional tests for the following tests:

-   Uncheck when user clicks the checkbox

-   Focus checkbox when user presses tab

-   Can you think of more accessibility tests?

Testing library works the same for all major frontend frameworks. So, a
Testing Library test written in Angular, can be re-used when migrating
to a different framework like React.

**Laws of UX**

When thinking about Usability there are many rules that could apply.
Laws of UX [^8] is a collection of easy-to-understand best practices for
designing user interfaces. It contains the take-aways from many
psychological studies about user experience. It\'s good to know about
the psychological laws the top apps use to keep you engaged in their
apps. For example, Doherty Threshold principle about the perceived
waiting experience of a user:

## Doherty Threshold [^9]

![A picture containing text, electronics Description automatically
generated](./media/image1.png)


1.  Provide system feedback within 400 ms in order to keep users'
    attention and increase productivity.

2.  Use perceived performance to improve response time and reduce the
    perception of waiting.

3.  Animation is one way to visually engage people while loading or
    processing is happening in the background.

4.  Progress bars help make wait times tolerable, regardless of their
    accuracy.

5.  Purposefully adding a delay to a process can increase its perceived
    value and instil a sense of trust, even when the process itself
    takes much less time.

It's nice to reference these principles when developers have created an
interface that is not usable, which I have seen a lot.

Tip: Always have a dedicated UX designer available when creating any
website. Preferably already in the initiation phase of a project.

**Visual Component Testing and Vite 3.0**

A big trend currently is visual component testing. Many popular
frameworks such as Cypress, Playwright and Storybook are now publishing
Beta support for this. Visual tests were usually very slow, flaky and/or
hard to maintain, but that has now changed through the power of Vite 3.0
[^10] (means \`fast\` in French). It is a build tool that aims to
provide a faster and leaner development experience for modern web
projects. It is revolutionising web development and making Webpack a
thing of the past.

Now tests are fast, and developers get very good visual feedback about
the state of their test. Also inspecting a browser and seeing logs in a
browser console for debugging is a lot better than interpreting the
Terminal test output and guessing the state a view is in.

I would currently suggest Cypress 10 Component Testing Beta. I think it
currently has the best developer experience compared to the other tools
and tests are very fast. It comes with a complete tool which makes it
easy to navigate through your test suite, re-run tests, debug tests, and
step through in-between steps of a specific test.

Tip: Try-out the Cypress 10 Component testing introduction [^11]

**Practical Usability Tests**

Ask a close relative or colleague to test your website. You will be
surprised how many things don\'t work well or are confusing to the user:

1.  Give them a goal to accomplish, like registering an account.

2.  Just observe them. Do not help, don\'t speak. They might be
    struggling, or not.

3.  Look at their non-verbal communication. A smile, a confused look,
    body language. This should give you enough information. Words are
    not that important.

Optional:

4.  Film their upper body and the corresponding screen recording to play
    back.

You should reach out to accessibility user groups including people with
disabilities that like to help with testing your website's
accessibility.

You could also hire a company that specializes in doing usability tests
with actual end-users of your website or people that fit a specific
persona.

The reason this works so well is that you are biased, and you went
through a specific flow on your site maybe ten to hundreds of times. Do
you remember being new to something and you spot everything that
doesn\'t work well?

Let's create a more accessible internet for everyone!

[^1]: <https://blog.hubspot.com/website/accessibility-statistics>

[^2]: <https://www.gartner.com/en/documents/3986300/compliance-and-beyond-4-ways-digital-accessibility-gives>

[^3]: <https://www.funkify.org/>

[^4]: [https://lawsofux.com/en/jakobs-law/]{.underline}

[^5]: <https://www.w3.org/WAI/ARIA/apg/patterns/>

[^6]: <https://chrome.google.com/webstore/detail/wave-evaluation-tool/jbbplnpkjmmeebjpijfedlgcdilocofh>

[^7]: <https://testing-library.com/>

[^8]: <https://lawsofux.com>

[^9]: <https://lawsofux.com/en/doherty-threshold>

[^10]: <https://vitejs.dev/guide/>

[^11]: <https://docs.cypress.io/guides/component-testing/writing-your-first-component-test>
