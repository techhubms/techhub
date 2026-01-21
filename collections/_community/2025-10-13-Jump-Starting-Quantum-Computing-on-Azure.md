---
external_url: https://techcommunity.microsoft.com/t5/azure-compute-blog/jump-starting-quantum-computing-on-azure/ba-p/4459053
title: Jump Starting Quantum Computing on Azure
author: notanaha
feed_name: Microsoft Tech Community
date: 2025-10-13 23:40:03 +00:00
tags:
- Azure Quantum
- Azure Quantum Development Kit
- Cloud Quantum Computing
- CNOT Gate
- Entanglement
- Hadamard Gate
- Python
- Qiskit
- Quantum Algorithm
- Quantum Circuit
- Quantum Computing
- Quantum Gates
- Quantum SDK
- Quantum Simulator
- Quantum Teleportation
- Quantum Workspace
- Qubit
- Shor's Algorithm
- Superposition
- VS Code
section_names:
- ai
- azure
- coding
---
notanaha explains quantum computing fundamentals and demonstrates, step by step, how to run the quantum teleportation protocol with real code on Azure Quantum using Python and Qiskit.<!--excerpt_end-->

# Jump Starting Quantum Computing on Azure

Quantum computers use the unique rules of quantum physics—superposition and entanglement—to process information in new ways. They won't replace today's laptops, but they open paths to supercharged tasks in search, optimization, materials science, and cryptanalysis. While the hardware is still in early stages and error-prone, progress is steady and filled with potential.

## Why Quantum, Why Now?

Quantum mechanics underlies much of today's technology (like semiconductors and MRIs), but quantum computing goes further: it uses quantum effects directly in computation. This article serves as a primer, giving you just enough math and physics to gain intuition, guides you through the quantum teleportation protocol, and shows how to test it hands-on with Azure Quantum.

## Classical bits vs. Qubits

- **Bit:** Always 0 or 1, like a coin landing heads or tails
- **Qubit:** Can be 0, 1, or any superposition—conceptually like a spinning coin not yet caught

**Tech stacks:**

- Classical: Transistors → logic gates → flip-flops & registers → instruction set → languages
- Quantum: Physical qubits (ions, superconductors, photons) → quantum gates (Hadamard, CNOT) → algorithms → SDKs (Qiskit, Q#, Cirq, Braket)

Quantum gates work on probability amplitudes and create correlations (entanglement) not efficiently possible with classical circuits.

## What is a Qubit?

A qubit is a two-state quantum system—conventionally written as |0⟩ (vector [1,0]^T) and |1⟩ (vector [0,1]^T). Measurement always yields 0 or 1, but before measurement, the state can be any linear combination of those two (superposition).

## Superposition and Entanglement

**Superposition:**

- Qubits can exist in a blend (superposition) of |0⟩ and |1⟩
- Visually: an arrow pointing on a sphere (the Bloch sphere); where it points determines the probabilities observed when measured

**Entanglement:**

- Strong quantum correlation between two or more qubits
- Example: a Bell state (1/√2)(|00⟩ + |11⟩)
- If one is measured as 0, the other is instantly 0 as well, regardless of distance

## Quantum Gates and Circuits

Quantum computers use quantum gates (like classical AND/NAND), but these gates act on superpositions and entangled states.

- **Hadamard (H) gate:** Creates superposition from |0⟩ or |1⟩
- **CNOT (Controlled-NOT):** Flips one qubit based on the state of another (creates entanglement)

**Example:** Apply H to qubit 1 (create superposition), then CNOT (entangles qubit 1 and 2).

## Quantum Teleportation Protocol

This protocol allows the transfer of an unknown quantum state from Alice to Bob. They start with a shared entangled pair. Alice acts on her qubits, measures, and sends two classical bits to Bob. Bob applies appropriate gates to his qubit, recreating Alice's original state.

- **Why important?** Demonstrates how quantum phenomena can be harnessed for practical information transfer, forming the basis for a future quantum internet and secure communication.

## Running Quantum Teleportation on Azure Quantum

1. **Set up Azure Quantum workspace:**
   - Use the Azure portal to create a workspace (free tier available)
   - Get your Resource ID and region
2. **Environment setup in VS Code:**
   - Install the Azure Quantum Development Kit extension
   - pip install: azure-quantum[qiskit] qsharp ipykernel matplotlib pylatexenc

3. **Writing and submitting the circuit (using Qiskit):**

```python
from azure.quantum import Workspace
from qiskit import QuantumCircuit
from qiskit.visualization import plot_histogram
from azure.quantum.qiskit import AzureQuantumProvider
import os
from dotenv import load_dotenv
load_dotenv(override=True)

workspace = Workspace(
    resource_id = os.getenv("RESOURCE_ID"),
    location = os.getenv("LOCATION")
)
provider = AzureQuantumProvider(workspace)
```

- Prepare a teleportation circuit with 3 qubits and 3 classical bits. Qubit 0: state to teleport, Qubit 1: Alice's entangled, Qubit 2: Bob's entangled.

```python
circuit = QuantumCircuit(3,3)
circuit.x(0)
circuit.barrier()
circuit.h(1)
circuit.cx(1,2)
circuit.barrier()
circuit.cx(0,1)
circuit.h(0)
circuit.barrier()
circuit.measure([0, 1], [0, 1])
circuit.barrier()
circuit.cx(1, 2)
circuit.cz(0, 2)
circuit.measure([2], [2])
circuit.draw(output='mpl')
```

- List available simulators:

```python
for backend in provider.backends():
    print("- " + backend.name())
```

- Pick a backend (e.g., "quantinuum.sim.h1-1e") and run the circuit:

```python
simulator_backend = provider.get_backend("quantinuum.sim.h1-1e")
job = simulator_backend.run(circuit, shots=1024)
result = job.result()
counts = {format(n, "03b"): 0 for n in range(8)}
counts.update(result.get_counts(circuit))
print(counts)
plot_histogram(counts)
```

- Analyze results: Successful teleportation puts Bob's qubit in the transferred state (look for output patterns like 100, 101, 110, 111 in Qiskit's convention).

## Conclusion

Quantum computing translates counterintuitive physical principles into real tools. This walkthrough explained foundational concepts, presented a classic protocol (teleportation), and gave you practical steps for trying it yourself with Azure Quantum. The field is rapidly evolving: as error rates drop and hardware matures, expect bigger breakthroughs in cryptography, optimization, and beyond. Stay tuned, as the next advances may come sooner than expected!

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/azure-compute-blog/jump-starting-quantum-computing-on-azure/ba-p/4459053)
