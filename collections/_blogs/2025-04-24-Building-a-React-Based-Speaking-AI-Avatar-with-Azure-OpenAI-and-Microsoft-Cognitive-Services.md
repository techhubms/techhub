---
layout: "post"
title: "Building a React-Based Speaking AI Avatar with Azure OpenAI and Microsoft Cognitive Services"
description: "This guide by Harald Binkle explains how to build a React application featuring an interactive AI avatar. It details integrating Azure OpenAI for conversation, Microsoft Cognitive Services for speech and viseme data, and avatar lip-syncing for a natural, engaging user experience."
author: "Harald Binkle"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://harrybin.de/posts/give-it-a-face-to-talk-to/"
viewing_mode: "external"
feed_name: "Harald Binkle's blog"
feed_url: "https://harrybin.de/rss.xml"
date: 2025-04-24 22:00:00 +00:00
permalink: "/2025-04-24-Building-a-React-Based-Speaking-AI-Avatar-with-Azure-OpenAI-and-Microsoft-Cognitive-Services.html"
categories: ["AI", "Azure", "Coding"]
tags: ["AI", "AnimationMixer", "Avatar", "Azure", "Azure OpenAI", "Blogs", "Coding", "GLTFLoader", "Lip Syncing", "Microsoft Cognitive Services", "React", "React Three Fiber", "Speech Integration", "Speech SDK", "SSML", "Three.js", "Viseme", "WebRTC"]
tags_normalized: ["ai", "animationmixer", "avatar", "azure", "azure openai", "blogs", "coding", "gltfloader", "lip syncing", "microsoft cognitive services", "react", "react three fiber", "speech integration", "speech sdk", "ssml", "threedotjs", "viseme", "webrtc"]
---

In this detailed guide, Harald Binkle demonstrates how to build a React-based AI avatar UI. The tutorial covers integrating Azure OpenAI, Microsoft Cognitive Services, and implementing avatar lip-syncing for truly interactive AI conversations.<!--excerpt_end-->

# Building a React-Based Speaking AI Avatar with Azure OpenAI and Microsoft Cognitive Services

**Author: Harald Binkle**

---

## Why Build a Speaking Avatar Instead of a Text-Based Chatbot?

While text-based chatbots serve many purposes, a speaking avatar enhances user engagement, accessibility, and emotional connection by enabling voice-driven, face-to-face-like interactions. This approach addresses several UX challenges, particularly for users who prefer or require auditory communication or a more natural, conversation-like flow.

### Key Benefits

1. **Enhanced User Engagement**: Combining voice and visual elements fosters a more immersive, human-like experience.
2. **Accessibility**: Bidirectional voice interactions support users with visual impairments or those unable to type.
3. **Emotional Connection**: Conveying emotion with voice and expression helps foster rapport, useful in fields like education, customer service, or mental health.
4. **Faster Communication**: Speaking typically transfers information faster than typing/reading.
5. **Brand Differentiation**: A speaking avatar can help your application stand out through innovation and user-centricity.

---

## Project Goal

**Create an interactive AI avatar in React that integrates:**

- Azure OpenAI Service for natural responses
- Microsoft Cognitive Services for speech-to-text, text-to-speech, and viseme-driven lip syncing
- Real-time, visually appealing 3D avatar rendering and synchronization

---

## Core Components

### 1. Microsoft Cognitive Services: Speech Integration

- **Speech-to-Text**: Transcribes user voice input.
- **Text-to-Speech**: Synthesizes AI-generated replies to spoken output.
- **Viseme Data**: Supplies lip synchronization data (phoneme-to-lip-shape mappings) for driving animations.

#### Flow

- Capture user voice via microphone
- Use SDK to transcribe to text
- Send user input (converted to text) to Azure OpenAI
- Receive AI response, synthesize to speech, and fetch viseme data
- Map viseme data to drive avatar's mouth movement

### 2. Azure OpenAI Integration

Azure’s GPT models generate context-aware conversational responses for the avatar. The flow involves:

1. User speaks → voice input transcribed to text
2. Azure OpenAI receives the message and returns an answer
3. Answer is converted to speech (and visemes) via MS Cognitive Services

### 3. Avatar Lip-Syncing

- Viseme data, streamed from Speech SDK, synchronizes the avatar’s mouth/face with the spoken output.
- Data is queued and processed in real time for smooth animation and synchronization.

### 4. React Application Integration

Managing state, user interaction, conversation flow, and visual rendering in a cross-platform React application.

---

## Addressing Technical Pain Points

- **Real-Time Synchronization**: Efficiently update avatar visuals with fresh viseme data while minimizing React re-renders.
- **Latency Management**: Minimize delays across speech recognition, AI response, and TTS pipelines.
- **Viseme to Animation Mapping**: Queue viseme events and process them to trigger correct blendshapes/animations.
- **Interrupting Speech Synthesis**: Handle user interruptions gracefully and stop both the audio output and animation as needed, despite SDK limitations.

---

## Example: Integrating MS Avatar in React

Microsoft provides ready-to-use avatars and example code via Speech Studio. This is the easiest way to get started with 3D avatars synchronized with MS Cognitive Services.

**Example React Integration:**

```js
import React, { useRef, useEffect } from "react";
import useMSAvatar from "./useMSAvatar";
import MSAvatarPanel from "./MSAvatarPanel";

function App() {
    const audioRef = useRef(null);
    const videoRef = useRef(null);

    const { load, unload, speakNext, stopSpeaking } = useMSAvatar();

    useEffect(() => {
        const avatarConfig = {
            talkingAvatarCharacter: "DefaultCharacter",
            talkingAvatarStyle: "DefaultStyle",
            greenscreen: false,
        };
        const speechConfig = {
            subscriptionKey: "YOUR_SUBSCRIPTION_KEY",
            region: "YOUR_REGION",
            voice: "en-US-JennyNeural",
            speakingStyle: "cheerful",
        };
        if (audioRef.current && videoRef.current) {
            load(avatarConfig, speechConfig, audioRef.current, videoRef.current);
        }
        return () => { unload(); };
    }, [load, unload]);

    const handleSpeak = () => {
        speakNext("Hello! I am your speaking avatar.");
    };
    const handleStop = () => {
        stopSpeaking();
    };

    return (
        <div>
            <MSAvatarPanel audio={audioRef} video={videoRef} />
            <button onClick={handleSpeak}>Speak</button>
            <button onClick={handleStop}>Stop</button>
        </div>
    );
}

export default App;
```

**MSAvatarPanel component:**

```js
interface MSAvatarPanelProps {
    audio: React.RefObject<HTMLAudioElement | undefined>;
    video: React.RefObject<HTMLVideoElement | undefined>;
}

function MSAvatarPanel({ audio, video }: MSAvatarPanelProps) {
    return (
        <Stack width="auto" height="100%" style="background: 'white'">
            <audio ref={audio as Ref<HTMLAudioElement>} id="audioPlayer" autoPlay />
            <video ref={video as Ref<HTMLVideoElement>} id="videoPlayer" autoPlay playsInline width="100%" height="100%" />
        </Stack>
    );
}

export default React.memo(MSAvatarPanel);
```

- **load** initializes the avatar and sets up WebRTC for media streaming.
- **speakNext** triggers speech output and animation synchronization.

---

## Using Custom Avatars

You can create custom avatars (e.g., [Avaturn](https://hub.avaturn.me/)), export them as `.glb` files, and load them in React using [Three.js](https://threejs.org/) or [react-three-fiber](https://github.com/pmndrs/react-three-fiber).

**Sample avatar loader:**

```js
import { Canvas } from "@react-three/fiber";
import { GLTFLoader } from "three/examples/jsm/loaders/GLTFLoader";
import { useEffect, useRef } from "react";

function Avatar() {
    const avatarRef = useRef(null);
    useEffect(() => {
        const loader = new GLTFLoader();
        loader.load("/path/to/avatar.glb", gltf => {
            avatarRef.current = gltf.scene;
        });
    }, []);
    return (
        <Canvas>
            {avatarRef.current && <primitive object={avatarRef.current} />}
        </Canvas>
    );
}
```

### Animation & Viseme Processing

- Use an `AnimationMixer` to control the avatar’s animations.
- Queue viseme data events and process them to update blend shapes/morph targets in sync with speech.
- Sample function to process the queue:

```js
async function processQueue(mixer) {
    const v = queue.current[0];
    if (v) {
        await processViseme(v, mixer);
        queue.current = queue.current.slice(1);
        if (queue.current.length > 0) {
            await processQueue(mixer);
        } else {
            setTimeout(() => {
                if (!idleAction.current && queue.current.length === 0) {
                    processIdle();
                }
            }, 200);
        }
    }
}
```

**processViseme** maps viseme data to blend shapes and triggers animation clips. If you interrupt speech, killing the SDK’s audio stream may require private/fragile workarounds:

```js
async function killSynthesizer() {
    if (synthesizer.current !== undefined) {
        console.log("Killing synthesizer");
        KillAudio(synthesizer.current);
        synthesizer.current.close();
        synthesizer.current = undefined;
    }
}

function KillAudio(synthesizer) {
    // Uses private SDK properties
    // @ts-ignore
    const audio = synthesizer.privAdapter?.privSessionAudioDestination?.privDestination?.privAudio;
    if (audio) {
        audio.pause();
        audio.currentTime = 0;
    }
}
```

A proper implementation ensures that visual and audio output stop in sync when the user interrupts.

---

## Conclusion

By leveraging Azure OpenAI and Microsoft Cognitive Services, developers can build truly engaging, conversational, and visually rich AI avatars in React applications. The key technical ingredients include real-time speech and viseme data, efficient animation control, and effective state management.

Future improvements could include emotion detection, more sophisticated animation, and personalized voice/appearance tuning.

**References:**

- [Microsoft Speech Studio](https://speech.microsoft.com/portal/3cf6c385d1e14645a0685128d3b96f23/livechat)
- [Avaturn Custom Avatars](https://hub.avaturn.me/)
- [Three.js](https://threejs.org/)
- [react-three-fiber](https://github.com/pmndrs/react-three-fiber)

*This site uses cookies for analytics (Microsoft Clarity).*

This post appeared first on "Harald Binkle's blog". [Read the entire article here](https://harrybin.de/posts/give-it-a-face-to-talk-to/)
