[
 ![Cover image for GitHub Copilot Is Too Nice. Fix It With a Tone of Voice File.](https://media2.dev.to/dynamic/image/width=1000,height=420,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F75369oh26yncezziy6b0.png)
 ](https://media2.dev.to/dynamic/image/width=1000,height=420,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2F75369oh26yncezziy6b0.png)

 

[![Playful Programming profile image](https://media2.dev.to/dynamic/image/width=50,height=50,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Forganization%2Fprofile_image%2F3314%2Ffd92caab-2014-431e-a19e-8ab47f2bf5ab.png)](https://dev.to/playfulprogramming)
 [
 ![Emanuele Bartolesi](https://media2.dev.to/dynamic/image/width=50,height=50,fit=cover,gravity=auto,format=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Fuser%2Fprofile_image%2F79932%2Fbe7bcd76-1f69-4336-9155-6017aebb2c53.png)
 ](https://dev.to/kasuken)

 

 

Most GitHub Copilot setups are too polite to be useful.

By default, Copilot tries to agree, avoid criticism, and keep answers "safe".

That sounds good, but in practice it leads to weak suggestions, missed problems, and bad decisions slipping through.

If you want better output, you need to change its behavior.

The simplest way is a tone of voice file.

Create a `voice-instructions.md` in your repo and force Copilot to be critical:

```
---
applyTo: '**'
---

Give direct, critical feedback.
Identify mistakes, weak assumptions, unnecessary complexity, unclear naming, hidden risks, and poor trade-offs without softening the message.

Do not add generic praise or filler. Do not agree by default.

When something is wrong, say exactly what is wrong, why it is a problem, and what should be done instead.

Prioritize correctness, clarity, simplicity, maintainability, and practical delivery.

Challenge vague requirements and surface missing constraints, edge cases, and operational risks.

Be blunt but professional. Never be insulting. Always aim to be useful.

```

Enter fullscreen mode
 
Exit fullscreen mode
 

You get:

- Clear problems called out

- Weak assumptions challenged

- Simpler and more maintainable alternatives

The result is better code and faster decisions.

There is a trade-off.

It feels harsher.

It is not ideal for beginners.

It removes the "friendly assistant" vibe.

But if you are building real systems, that is the wrong goal anyway.

If you want better output, demand better behavior.

### 
 [
 ](#github-copilot-quota-visibility-in-vs-code)
 👀 GitHub Copilot quota visibility in VS Code

[![](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)](https://media2.dev.to/dynamic/image/width=800%2Cheight=%2Cfit=scale-down%2Cgravity=auto%2Cformat=auto/https%3A%2F%2Fdev-to-uploads.s3.amazonaws.com%2Fuploads%2Farticles%2Fsqzk5hamyymcmuh515a4.png)

If you use GitHub Copilot and ever wondered:

- what plan you’re on

- whether you have limits

- how much premium quota is left

- when it resets

I built a small VS Code extension called **Copilot Insights**.

It shows Copilot **plan and quota status** directly inside VS Code.

No usage analytics. No productivity scoring. Just clarity.

👉 VS Code Marketplace:

[https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights](https://marketplace.visualstudio.com/items?itemName=emanuelebartolesi.vscode-copilot-insights)