# How API Thinking revolutionizes Healthcare

**Have you visited a hospital recently? You probably noticed that the
hospital didn't feel high tech. Nurses, doctors and supporting staff
often look like data-entry engineers. The user interfaces they work with
are old fashioned and their computers are slow. Let's face reality: our
standard is the digital enterprise, while many hospitals are still
living in the stone age of digitalization. Luckily there's a movement
which rapidly changes hospitals: FHIR (pronounced: FIRE)!**

To understand where this hospital IT backlog comes from, you'll need to
take a deeper look into the processes in healthcare. Treatment of a
patient hardly ever originates from the hospital itself. Typically, it
starts from a general practitioner (local doctor) who creates an initial
observation. This observation leads to a referral to a more specialized
practitioner. This could either be in a hospital or in a specialized
clinic. When this specialist practitioner tries to perform diagnosis,
additional studies such as Imaging or Laboratory Studies are necessary
most of the time. The specialist can\'t perform this study himself, so,
he'll create another referral. Once this study is completed, the
specialist receives details about the study and determines a treatment
plan. Part of this plan is a referral to a more specialized
practitioner. And so the journey continues\... Each disease and each
human body is different, and thus deserves its own, tailor made
treatment. All of the steps in these custom workflows collect and
transfer information.

It gets more complex when you realize that not all specialisms are
working inside the same hospital, building or legal entity. Treatments
are sometimes even stretched across country borders. Each specialized
practitioner comes in with his own processes, tooling and jargon. In the
IT industry we learned the Conway Law,[^1] that communication structures
are reflected in the systems you design. Hospital IT is no exception to
this. Every specialism has its own system, with its own technology and
characteristics. Many of these specialisms aren\'t really part of the
hospital; they are associated with the hospital as a separate legal
entity, while isolating patient data inside their own systems.

During recent years, large regulatory bodies such as the E.U. have
started to form opinions about data ownership. With the intention of
protecting civilians against the influence large companies can have on
their lives, new protection regulation has been written. Most people
have heard about the GDPR, but don't understand the enormous impact on
various industries. Inside the healthcare industry a patient already has
the right to have a look at his dossier with a summary of the various
treatments, but under the new regulations, everyone has obtained the
right to change, correct, transfer or erase all information about
himself, at no cost (within certain legal boundaries).

To simplify the handover in the workflows, hospitals have already
started automating using Hospital Information Systems (HIS). These are
large central systems collecting data about patients. Typically, a HIS
contains central appointment management, patient dossiers and workflow
registration. Since these systems are responsible for the overview, they
only connect to more specialized systems via hyperlinks or application
launchers.

When your treatment is inside a single hospital, the HIS typically does
its job very well. But once we go beyond the hospital, for example for a
second opinion, you immediately hit a wall. The need to share the entire
(relevant) patient history, including all of its specialized studies, is
a complex question. There's a clear need for a smart way of
interoperability between the two parties.

In the late 80s, the HL7 standard organization was founded. This
organization has a clear mission statement: A world in which everyone
can securely access and use the right health data when and where they
need it. As a result, several standards were published. Unfortunately,
the adoption of these standards is relatively slow. In recent years, as
the business case started to increase, HL7 made a strategic move, and
brought together a group of very active community members.[^2]

FHIR stands for Fast Healthcare Interoperability Resources and is
basically a domain definition for a REST API. It has abstracted all
functionality in building blocks, which can evolve independently. The
domain objects use ISO-standards for data formatting and can be queried
in a traditional REST way or using GraphQL. The standard contains the
latest generation security (OAuth), has an answer for object
extensibility, and allows for searching.

Probably the most powerful feature of FHIR R4 is the clear way in which
capabilities are structured and secured. The API is transparent about
which pieces of the specification are implemented, and how they are made
available. Any implementer of the standard only needs to expose the
relevant parts. While a consumer can use the API to quickly find out
whether a capability is available at all.

Now let's picture a use case in which an AI startup specializes in
analyzing complex bone fractures. For a hospital, this can be an
enormous cost saver as it enables quicker diagnoses. Using their FHIR
endpoint, all they have to do is expose an Imaging Studies-capability to
this startup. The startup can then read the patient scans (Imaging
Studies) from the endpoint. Once the analysis is completed, an
observation is POST-ed back to the hospital. This observation can lead
to a formal diagnosis once a specialist in the hospital has approved it.

From an architectural perspective the FHIR standard also opens doors. By
having a clear way of showing how systems should communicate with each
other, the FHIR API decouples all these systems from each other. It
allows for applying Sacrificial Architectural[^3] principles on the
hospital enterprise architecture. Think about migrating data from a
legacy system to a new one, consolidate two systems into one system, or
even seamlessly replace one system with another.

Several large cloud vendors - including Microsoft, Amazon and Google --
are creating support in their platforms for FHIR.[^4] Of course,
healthcare is one of the largest industries in the world, but besides
the financial advantage, they also see FHIR's contribution to world
health. The support of these giants guarantees technical innovation in
the long run.[^5][^6]

In addition to the adoption by the cloud vendors, you can see an active
community creating several innovations and standards related to FHIR R4.
Large, industry-wide hackathons such as the FHIR DevDays[^7] are
delivering great value and new, unforeseen insights. A great example of
this is the development of SMART-on-FHIR, which allows applications to
start in the context of a specific user. How cool would it be if the
assistant can send the right context to the active mobile device the
doctor is currently carrying?

Other industries can learn from the steps the HL7 FHIR team is taking.
For example, the domain model wasn\'t built with the purpose of
justifying existing systems. Instead, it was developed as part of the
community process. The intention of this technology push is clear: it
enables a larger audience to adopt FHIR quicker. Above all, the
commitment of the community is unprecedented. The delivery FHIR R5 has
already started and is expected to be ready in 2020[^8].

Back in 2013, Harvard Business Review published a clear vision on the
future of healthcare.[^9] The widely recognized Harvard professor Porter
stated that decentralized treatment and patient-centered data management
(among a few others) could 'fix' healthcare. FHIR and its community are
an enabler for both. With the support of the tech giants and an emerging
need due to population growth, healthcare takes the next step. It might
be slower than other industries, but eventually we'll all benefit from
this technical innovation.

[^1]: <https://en.wikipedia.org/wiki/Conway%27s_law>

[^2]: https://www.hl7.org/fhir/

[^3]: https://martinfowler.com/bliki/SacrificialArchitecture.html

[^4]: https://www.geekwire.com/2019/microsoft-amazon-tech-giants-forge-ahead-healthcare-data-sharing-pledge/

[^5]: https://azure.microsoft.com/en-us/services/azure-api-for-fhir/

[^6]: https://github.com/microsoft/fhir-server/

[^7]: https://www.devdays.com/

[^8]: https://onfhir.hl7.org/2019/01/20/fhir-r5-roadmap/

[^9]: https://hbr.org/2013/10/the-strategy-that-will-fix-health-care
