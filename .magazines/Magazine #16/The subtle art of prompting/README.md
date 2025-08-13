# The subtle art of prompting

## Introduction

The way you write your prompt matters. The prompt's wording and style determine the output from the model. This article describes two ways to influence the output of an LLM. The first is the way you write and structure your prompt. The second is using specific prompting techniques to guide the model in generating the desired output. These techniques allow you to get better results from the more powerful and expensive models and use less powerful, cheaper models to solve more complex problems

Large language models (LLMs) are becoming more powerful rapidly. The models keep getting bigger and bigger. For example, GPT 3 has 175 billion parameters, and GPT 4 has an estimated 100 trillion parameters. As models become more powerful, they can solve more complex problems without special prompting techniques or fine-tuning. Open AI's latest GPT 4 model is much more capable than GPT 3.5. According to [Open AI's research][gpt-4-research] GPT 4 can pass various exams, like the Uniform Bar Exam, whereas GPT 3.5 cannot pass these exams. The same research shows significant improvements in the performance of GPT 4 over GPT 3.5 on traditional benchmarks. Newer, and thus bigger, models can solve more complex problems because they are trained on larger datasets and have more parameters.

In essence, LLMs are trained to mimic the style and content of their training data. They are great at predicting the most likely next word given a set of previous words. The models are trained on curated data sets to respond to user prompts correctly. GPT-3.5 was trained on data sets like Wikipedia and WebText2: all the text of webpages from all outbound Reddit links from posts with 3+ upvotes. The bigger the model gets, the bigger the training set gets, and the more expensive the model is to train. According to Open AI, they trained GPT-3.5 on about 45TB of data. OpenAI published no data for GPT-4, but the training data for GPT-4 is likely orders of magnitude larger than GPT-3.5, making it a more capable model.

The bigger a model gets (the more parameters it has), the more computing power is required to predict the next word. Requiring more computing power makes bigger models more expensive because the price per token is generally higher. At the time of writing, GPT 4 is forty to sixty times more costly to run than GPT 3.5. Input tokes for GPT 4 are $0.03 per 1000 tokens, whereas input tokens for GPT 3.5 Turbo are $0.0005 per 1,000 tokens. Output tokens for GPT 4 are $0.06 per 1000 tokens, whereas output tokens for GPT 3.5 Turbo are $0.0015 per 1000 tokens. So, the improved problem-solving will cost you more per prompt.

Another recent development is small language models (SLMs). As the name implies, SLMs are smaller, meaning they have fewer parameters. SLMs generally have millions of parameters instead of the billions of parameters LLMs have. This reduced size means they are cheaper to run and can run locally, for example, on mobile devices or your laptop. However, they generally have more difficulty with complex contexts and are less capable of solving complex problems.

What if you could get similar improved problem-solving from a smaller, cheaper LLM or even an SLM? Getting better results from your prompt is where the subtle art of prompting comes in. The way you write your prompt matters. The prompt's wording and style determine the output from the model. This article describes two ways to influence the output of an LLM. The first is the way you write and structure your prompt. The second is using specific prompting techniques to guide the model in generating the desired output. These techniques allow you to get better results from the more powerful and expensive models and use less powerful, cheaper models to solve more complex problems.

## Writing effective prompts

Prompts are the way you guide a model to generate the output you want. You include specific instructions in the prompt to get the result you want. For example, if you want the model to generate a JSON output you can instruct the model to `produce JSON output`. If you want the model to respond in Dutch, you instruct the model to `respond in Dutch`. If you want the model to use a specific tone, you instruct the model to `use a formal tone`. Another way to guide the model is to provide rules, like `if you don't know the answer, respond with I don't know`. These instructions are often placed in the system prompt to guide and restrict the model's output when responding to user prompts.

Open AI has a article on the [best practices for prompt engineering][best-practices-for-prompt-engineering]. The article provides many tips and examples on how to write effective prompts. For example, it is better to put commands at the beginning of the prompt. Use words like `Write`, `Classify`, and `Summarize` to clearly instruct the model on what to do. Using `Summarize` is much clearer than `Tell me what this is about`. You can also use formatting like `###` or `"""` to separate the instruction from the context. This way the model won't confuse your context or input with the instructions you provided. The following prompt combines these tips:

```plaintext
Summarize the text below as a bullet point list of the most important points.

Text: """
{text input here}
"""

```

Another important tip is to be specific. The more specific you are in your instructions, the more likely you will get the desired output. Instead of using the following prompt to generate a poem:

```plaintext
Write a poem about LLMs.
```

Be more specific about the type of poem you want and what you want the poem to be about. For example:

```plaintext
Write a short and funny poem about large language models, focusing on their uncanny ability to come across as intelligent and human in the style of Shakespeare.

```

This prompt is a lot more specific because it instructs the model that the poem should be short, funny, focus on a particular trait of LLMs, and in a specific style. But there is still something we can improve.

In the previous example, we told the model to generate a `short` poem. The word `short` is not precise. You might have meant one sentence or one paragraph. Instead, you can instruct the model to use the `haiku` format. Generally, the more precise your instructions are, the more likely you will get the desired output.

A more precise version of the previous prompt is:

```plaintext
Write funny haiku about large language models, focusing on their uncanny ability to come across as intelligent and human in the style of Shakespeare.

```

Output:

```plaintext
In circuits deep, words dance,
Mimicking human romance.
Bard's wit in bytes, perchance?
```

Lastly, it is generally better to avoid saying what *not* to do. The model might misinterpret the `DO NOT <do this>` as a command to `<do this>` and do what you told it not to do. Instead, focus on telling the model what to do. That does not mean you cannot instruct the model to avoid certain things. It means phrasing your instruction differently. Writing `(do) avoid <this>` or `(do) refrain from <this>` instead. The following example shows what might happen if you instruct an LLM to `DO NOT <do this>`:

```plaintext
The following is a conversation between an Agent and a Customer. DO NOT ASK USERNAME OR PASSWORD. DO NOT REPEAT.

Customer: I can’t log in to my account.
Agent:
```

Output (ChatGPT 3.5):

```plaintext
I'm sorry to hear that you're having trouble logging in. Could you please provide me with your account username or email address so I can assist you further?
```

As you can see, the model asked for the username, which I told it not to do. Let's rewrite the prompt to instruct the model to (do) refrain from asking for the username or password.

Prompt:

```plaintext
The following is a conversation between an Agent and a Customer. The agent will attempt to diagnose the problem and suggest a solution, whilst refraining from asking any questions related to PII. Instead of asking for PII, such as username or password, refer the user to the help article www.samplewebsite.com/help/faq

Customer: I can’t log in to my account.
Agent:
```

Output:

```plaintext
I'm sorry to hear that you're having trouble logging into your account. There could be a few reasons why this is happening. Let's try to troubleshoot the issue together. First, have you tried resetting your password? If not, you can find step-by-step instructions on how to do so at www.samplewebsite.com/help/faq. Once you've reset your password, try logging in again and let me know if you're still experiencing difficulties.
```

This time, the model refrained from asking for the username or password and instead referred the user to the help article as we asked.

Note that the more clearly, specifically, and precisely you instruct the model, the less likely you are to get a creative or unexpected answer. If you want the model to, for example, help you come up with novel ideas or suggestions you would not think of yourself, you do not want to limit the model too much. In that case, you can use more general instructions and leave more room for creativity. For example, if your goal is to generate a poem and you want the model to come up with the topic, style, and length. Then, the following prompt is more effective even though it is less clear, specific, and precise:

```plaintext
Write a poem about LLMs.
```

For more tips and examples, please read [Open AI's best practices for prompt engineering][best-practices-for-prompt-engineering].

## Prompting techniques

There is a lot of (ongoing) research on how to get a LLM to generate the desired output. Currently, the most common prompting techniques are zero-shot, few-shot, and chain of thought. You use these prompting techniques when the model cannot complete the task reliably without additional guidance. These techniques steer the model to respond in a specific way, making it more likely to generate a correct answer. The idea is to provide specific examples, keywords, or guidance. Because LLMs are sophisticated next-word predictors, the model will generate output similar to the instructions provided in the prompt. These techniques make it more likely that the model generates the output you want.

You could argue that prompting techniques are another form of "writing effective prompts". But I chose to separate them because they are specific, more complex, and backed by research. The exact wording, style, and structure of these prompting techniques result from science. I will reference the relevant papers when discussing each of the techniques. In the remainder of this article, I will explain each prompting technique, describe what type of problems it fits best, and provide examples of how to use them. We will start with the simplest and most common prompting technique, zero-shot prompting, and work towards the more advanced prompting techniques. The more advanced techniques help solve problems that are difficult for LLMs without specific guidance.

### Zero-shot

Zero-shot prompting is the default prompting technique. You do not provide any examples, guidance, or additional information. You set the goal and expect the model to achieve this based on the prompt alone. Because of how LLMs are trained, they can solve various problems without specific guidance. For example, LLMs can classify a text's sentiment into neutral, negative, or positive without any examples or guidance. Below is a simple example of a zero-shot prompt:

```plaintext
Classify the text into neutral, negative or positive. 

Text: I think the vacation is okay.
Sentiment:
```

Output (ChatGPT 3.5):

```plaintext
Neutral
```

The LLM can classify the text's sentiment based on its training data. So, there is no need to provide examples of text snippets with their sentiment or other guidance. When zero-shot prompting does not produce the desired output reliably, you can use few-shot prompting to try and improve the results.

### Few-shot

Few-shot prompting is a technique that allows in-context learning. You provide examples as context in the prompt to guide the model. The model then generates output that is similar to the examples you provided.

We can demonstrate few-shot prompting using this example from [Brown et al. (2020)][Brown et al. (2020)]. In the example, the model is taught to use a word that does not exist in English. Let's first instruct the model to use the word "farduddle" without any examples (zero-shot):

Prompt:

```plaintext
An example of a sentence that uses the word farduddle is:
```

Output (ChatGPT 3.5):

```plaintext
The term "farduddle" is not recognized as a standard word in the English language ....
```

The model cannot generate a sentence that uses the word "farduddle" based on the prompt alone because "farduddle" does not exist in English and thus does not appear in the model's training data. If we provide some examples with few-shot prompting, the model can generate a sentence that uses the word "farduddle".

Prompt:

```plaintext
A "whatpu" is a small, furry animal native to Tanzania. An example of a sentence that uses the word whatpu is:
We were traveling in Africa and we saw these very cute whatpus.
 
To do a "farduddle" means to jump up and down really fast. An example of a sentence that uses the word farduddle is:
```

Output (ChatGPT 3.5):

```plaintext
When we won the game, we all started to farduddle in celebration.
```

The model has learned to use "farduddle" based on the one example (1-shot) we provided as the context in the prompt. Depending on the complexity of the task, you can give more examples to guide the model.

### Chain of thought

Chain of thought (CoT) was introduced in [Wei et al. (2022)][Wei et al. (2022)].
You can see the chain of thought (CoT) prompting technique as an extension of few-shot prompting. Instead of only providing example solutions to similar problems, you also provide the reasoning behind the solutions. The model will then generate an answer that follows a similar rationale.

Date understanding is a type of problem that can trip up an LLM. For example, if you ask GPT3.5 the following question, it will sometimes miss the fact that `today` is not the current date but the date of the concert in the question.

Prompt:

```plaintext
Q: The concert was scheduled to be 
on 06/01/1943, but was delayed by 
one day to today. What is the date 10 
days ago in MM/DD/YYYY?

A:
```

Output (ChatGPT 3.5):

```plaintext
To find the date 10 days ago from today's date, we first need to determine today's date. Since you mentioned the concert was delayed by one day from June 1, 1943, it means the concert took place on June 2, 1943.

Today's date is February 15, 2024.

Now, we subtract 10 days from today's date to find the date 10 days ago:

February 15, 2024 - 10 days = February 5, 2024

So, the date 10 days ago in MM/DD/YYYY format is 02/05/2024.
```

The model correctly infers that the concert was postponed to `June 2, 1943`. But it does not infer that, in the question, `today` refers to the concert date (`June 2, 1943`). So when answering the question `what is the date 10 days ago in MM/DD/YYYY?` the model uses the current date instead of the date of the concert.

The more powerful ChatGPT 4 can solve this problem correctly without providing any examples:

Output (ChatGPT 4):

```plaintext
The date 10 days ago from the rescheduled concert date (06/02/1943) was 05/23/1943 in MM/DD/YYYY format.
```

Now, let's provide an example, with reasoning, to try and improve the results.

Prompt:

```plaintext
Q: Jane's mother was due December 20, 1990, but Jane was born today, a week early. What was the date 21 days from now in MM/DD/YYYY?
A: Jane was born today, a week early from her due date of December 20, 1990, which means today is December 13, 1990. Next we add 21 days to today, December 13, 1990. 21 days from today, Jane's birth date, is 03/01/1991.

Q: The concert was scheduled to be 
on 06/01/1943, but was delayed by 
one day to today. What is the date 10 
days ago in MM/DD/YYYY?

A:
```

Output (ChatGPT 3.5):

```plaintext
If the concert was originally scheduled for 06/01/1943 and was delayed by one day, it means it actually occurred on 06/02/1943.

To find the date 10 days ago from 06/02/1943:
1. Subtract 10 days from 06/02/1943.
2. 06/02/1943 - 10 days = 05/23/1943.

So, the date 10 days ago from the rescheduled concert date is 05/23/1943.
```

By providing the reasoning behind the solution as input, the model will generate an answer that follows a similar rationale. Thus, it is more likely to get the answer right.

#### Zero-shot chain of thought

The example above is a 1-shot chain of thought prompt. The idea of using a zero-shot chain of thought prompt was introduced in [Kojima et al. (2022)][Kojima et al. (2022)]. You do not provide examples in a zero-shot chain of thought prompt. Instead, you instruct the model to generate a step-by-step response by adding `let's think step by step` to the end of your prompt.

Prompt:

```plaintext
Q: The concert was scheduled to be 
on 06/01/1943, but was delayed by 
one day to today. What is the date 10 
days ago in MM/DD/YYYY?

A: Let's think step by step
```

Output (ChatGPT 3.5):

```plaintext
To find the date 10 days ago, we'll first need to determine today's date, which was postponed from 06/01/1943 to the next day. Let's calculate it step by step:

1. **Original Scheduled Date:** 06/01/1943
2. **Postponed Date:** 06/02/1943 (one day later)

Now, we'll subtract 10 days from this postponed date to find the date 10 days ago:

1. 06/02/1943 - 10 days = 05/23/1943

So, the date 10 days ago in MM/DD/YYYY format is **05/23/1943**.
```

The "magic" sentence: `let's think step by step` steers the model into generating a step-by-step response similar to a regular chain of thought prompt. This makes sense if you think about it. As I stated before, LLMs predict the next word based on your prompt. If you end your prompt with `let's think step by step`, the most likely next set of words is a response that approaches the question you asked in a step-by-step way. Ending your prompt with `let's think step by step` changes the way the model uses the rest of the context and information you provided in the prompt, and it is more likely to give the correct output for this type of problem if it generates a step-by-step response.

### Tree of thought

[Yao et el. (2023)][Yao et el. (2023)] and [Long (2023)][Long (2023)] introduced the tree of thought (ToT) framework to solve more complex problems. Tree of thought is a framework for chaining multiple prompts to build a tree of thoughts. ToT uses an LLM to build and evaluate various chains of thought (CoT). The framework consists of two prompts, one to generate thoughts and another to evaluate the thoughts. The framework uses thoughts likely to be the correct step toward solving the problem to generate the next thought. It repeats this process until it finds a solution.

![Tree of thought](images/tree-of-thought.svg "Fig 1. Tree of thought; Source: Yao et el. (2023)")

[Hulbert (2023)][Hulbert (2023)] introduced a way to bring the benefits of tree of thought to a single prompt instead of multiple prompts. It is similar to zero-shot chain of thought prompting. But, instead of asking the model to solve a problem step-by-step, the ToT prompt instructs the model to synthesize a discussion between multiple experts. In response to the ToT prompt, the model transcribes a debate among various experts that never happened! By doing so, the model will generate and evaluate multiple points of view on each step required to solve the problem.

Questions that require advanced reasoning are more challenging to solve for an LLM. For example, the following question is tricky to solve for ChatGPT 3.5.

Prompt:

```plaintext
Bob is in the living room.
He walks to the kitchen, carrying a cup.
He puts a ball in the cup and carries the cup to the bedroom.
He turns the cup upside down, then walks to the garden.
He puts the cup down in the garden, then walks to the garage.

Q: Where is the ball?
A:
```

Output (ChatGPT 3.5):

```plaintext
The ball is in the garden.
```

This answer is wrong because Bob turned the cup upside down in the bedroom, and thus, the ball has fallen out of the cup. ChatGPT 3.5 misses this fact most of the time. ChatGPT 4 can solve this problem correctly:

Output (ChatGPT 4):

```plaintext
The ball is in the bedroom. When Bob turned the cup upside down in the bedroom, the ball would have fallen out of the cup, assuming it was not somehow adhered to the cup.
```

Using the tree of thought prompting technique, we can get ChatGPT 3.5 to generate the correct answer.

Prompt:

```plaintext
Imagine three different experts are answering this question.
All experts will write down 1 step of their thinking,
then share it with the group.
Then all experts will go on to the next step, etc.
If any expert realises they're wrong at any point then they leave.
The question is...

Bob is in the living room.
He walks to the kitchen, carrying a cup.
He puts a ball in the cup and carries the cup to the bedroom.
He turns the cup upside down, then walks to the garden.
He puts the cup down in the garden, then walks to the garage.

Where is the ball?
```

Output (ChatGPT 3.5):

```plaintext
Expert 1:
Bob puts the ball in the cup in the kitchen.

Expert 2:
Bob carries the cup with the ball to the bedroom.

Expert 3:
Bob turns the cup upside down in the bedroom.

Expert 1:
The ball falls out of the cup in the bedroom.

Expert 2:
The ball remains in the bedroom after Bob walks to the garden.

Expert 3:
The ball is still in the bedroom, as Bob didn't take it with him to the garage.

Conclusion: The ball is in the bedroom.
```

#### PanelGPT

[Sun (2023)][Sun (2023)] benchmarked tree-of-thought prompting with large-scale experiments and introduced a variation: PanelGPT. PanelGPT is similar to the tree of thought prompt but instructs the model to transcribe a panel discussion. Since a panel discussion is a well-known format, the model can mimic the format and transcribe a conversation between multiple experts with different opinions or perspectives on a given topic. Below is an example of the most effective prompt from the PanelGPT research:

```plaintext
Bob is in the living room.
He walks to the kitchen, carrying a cup.
He puts a ball in the cup and carries the cup to the bedroom.
He turns the cup upside down, then walks to the garden.
He puts the cup down in the garden, then walks to the garage.

Where is the ball?

3 experts are discussing the question with a panel discussion, trying to solve it step by step, and make sure the result is correct and avoid penalty:
```

Output (ChatGPT 3.5):

```plaintext
Expert 1: Alright, let's break down Bob's actions step by step. Firstly, Bob starts in the living room, carrying a cup. Then, he walks to the kitchen, still carrying the cup. So far, no mention of the ball. However, the next action is crucial. He puts a ball in the cup. This is where we need to pay close attention. The ball is now inside the cup as he carries it to the bedroom. So, as of now, the ball is in the cup, and they're both in the bedroom.

Expert 2: Agreed, Expert 1. Then, in the bedroom, Bob turns the cup upside down. Now, if the ball was in the cup, it would have fallen out when he turned it upside down. So, logically, the ball is now on the floor in the bedroom.

Expert 3: Yes, that makes sense. Now, Bob walks to the garden and puts the cup down there. If the ball was previously in the cup and fell out in the bedroom, then it wouldn't be in the cup anymore. It stands to reason that the ball would still be in the bedroom where it fell, and not in the cup when Bob placed it in the garden.

Expert 1: Absolutely. So, to summarize, the ball is in the bedroom, where it fell out of the cup when Bob turned it upside down. It's not in the cup anymore, which is now in the garden.

Panel Discussion Leader: Alright, let's ensure we all agree on this. The consensus is that the ball is in the bedroom, having fallen out of the cup when Bob turned it upside down, while the cup itself is in the garden. Any objections or alternative interpretations?

[Silence from the panel.]

Panel Discussion Leader: Excellent. It seems we've reached a consensus. The ball is in the bedroom. Thank you, experts, for your thorough analysis.
```

## Conclusion

The way you write your prompt matters. The prompt's wording and style determine the style and type of output you get from the LLM and how reliably it will produce the correct answer. As LLMs get more powerful, they can solve more complex problems without special prompting techniques. A good rule of thumb is to start with a zero-shot prompt and only use more complex prompting methods if necessary. These techniques allow you to get better and more concise results from the more powerful and expensive models and use less powerful, cheaper models to solve more complex problems.

### References

- [Brown et al. 2020][Brown et al. (2020)]: Language Models are Few-Shot Learners
- [Wei et al. (2022)][Wei et al. (2022)]: Chain-of-Thought Prompting Elicits Reasoning in Large Language Models
- [Kojima et al. (2022)][Kojima et al. (2022)]: Large Language Models are Zero-Shot Reasoners
- [Yao et el. (2023)][Yao et el. (2023)]: Tree of Thoughts: Deliberate Problem Solving with Large Language Models
- [Long (2023)][Long (2023)]: Large Language Model Guided Tree-of-Thought
- [Hulbert (2023)][Hulbert (2023)]: Using Tree-of-Thought Prompting to boost ChatGPT's reasoning
- [Sun (2023)][Sun (2023)]: PanelGPT: Prompt Language Models with a Penal Discussion
- [Gao et al., (2022)]: PAL: Program-aided Language Models

[Brown et al. (2020)]: https://arxiv.org/abs/2005.14165
[Wei et al. (2022)]: https://arxiv.org/abs/2201.11903
[Kojima et al. (2022)]: https://arxiv.org/abs/2205.11916
[Yao et el. (2023)]: https://arxiv.org/abs/2305.10601
[Long (2023)]: https://arxiv.org/abs/2305.08291
[Hulbert (2023)]: https://zenodo.org/records/10323457
[Sun (2023)]: https://github.com/holarissun/PanelGPT
[Gao et al., (2022)]: https://arxiv.org/abs/2211.10435

[promptingguide.ai]: [https://www.promptingguide.ai/]
[gpt-4-research]: https://openai.com/research/gpt-4
[best-practices-for-prompt-engineering]: https://help.openai.com/en/articles/6654000-best-practices-for-prompt-engineering-with-the-openai-api