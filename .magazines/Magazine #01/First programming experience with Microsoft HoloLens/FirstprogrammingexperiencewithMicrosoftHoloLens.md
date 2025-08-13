# First programming experience with Microsoft HoloLens

## Introducing HoloLens

Back in January of 2015 Microsoft unveiled the HoloLens, which it calls
the first holographic computer running Windows 10. The computer fitted
inside a wearable device will add holograms to the real physical world.
Holograms are objects made entirely of light, can be 2-dimensional or
3-dimensional and can be viewed from every side. They do not actually
exist, but appear to be blended into the real world. Unlike actual
objects they have no mass and cannot be touched. Even so, the experience
that the HoloLens creates provides a new view of the world.

![](./media/image1.png)


During the //build/ conference in April 2015 more details and
applications of the HoloLens were unveiled. Also, the Windows
Holographic Platform was announced as the higher level initiative to
integrate holograms into the Windows platform, where the HoloLens is a
hardware device that uses these capabilities.

A select group of developers were allowed to experience the HoloLens
first hand and create a working application for the device. Marcel de
Vries and Alex Thissen took the deep dive and worked their way through
the Microsoft Holographic Academy guided by Microsoft mentor Emily.

## New capabilities offer new possibilities

The HoloLens is an untethered wearable computer that fits on your head
with a band. It is like wearing a baseball cap with a large set of
sunglasses attached to it. Its band is secured with a knob on the back
of the band to make it an exact fit for your head. You can extract and
raise the visor to have it fit correctly on your head regardless of
whether you are wearing glasses or not. The visor contains multiple
sensors, a set of high-definition lenses to display the holograms and a
Holographic Processing Unit (HPU) to perform data acquisition and
processing of the sensor data. Before you use the HoloLens you need to
calibrate it so it knows the distance between the pupils of your eyes.
This was required on the developer devices we used, but it might be
possible this calibration step is not required in the final consumer
product.

The holographic images are overlaid on what you would see without
wearing the HoloLens. It makes it different from other 3D viewing
devices that occlude your vision totally, such as the Oculus Rift. This
enables you to interact with the real world instead of a fictional
world, opening up a whole set of additional possibilities. The HoloLens
not only offers visual enhancements, but also positional stereo sound by
two speakers on either side of the device.

The HoloLens is a device that has a vast array of sensors and devices
fitted inside the headset. The exact type of sensors has not been
disclosed yet, but they allow the HoloLens to support the following:

-   **Voice recognition**

You can issue voice commands that are picked up by the microphones
inside the HoloLens. The commands are recognized and translated to
string-based commands. These commands work amazingly accurate and
direct. You do not have to raise your voice, as the device picks up
normal spoken words and sentences.

-   **Spatial mapping**\
    The HoloLens can scan the environment (e.g. the room in which you
    are standing) and map out the objects and surfaces that it contains.
    Normally you would not see the mapping of the room. It is possible
    though, to show the mapping with a triangular mesh, so you can
    actually see what has been scanned. The scan of the environment is
    cached in some form, so you can quickly turn around and see the
    correct mapping. A rescan is performed periodically to update any
    changes, and also to remove any glitches that might have occurred.

-   **Gesture recognition**\
    The HoloLens is able to see your hands in the field of view. It can
    recognize (at least) a closed fist and an index finger. Moving your
    stretched index finger down and up is a "tap"-gesture, which is
    mostly used to indicate and initiate an action that would normally
    be done with a mouse-click. You can compare this to the capabilities
    the Microsoft Kinect introduced a couple of years ago.

-   **Gaze tracking**\
    Your gaze is comparable with a laser beam coming from the front of
    the HoloLens. It is centered in your field of view. By turning your
    head and body you can look around the entire environment redirecting
    your gaze by doing so. The center point of your view acts as the
    focus point for cursors projected into the environment. By moving
    your head and looking towards objects you gaze towards certain
    objects that can be detected by the Hololens. This enables you to
    select those objects and interact with them using gestures or voice
    commands.

-   **Ambient and positional sound**

The sound speakers in the HoloLens can produce both ambient sounds, like
background noises or music, and positional sound that appears to be
coming from a specific source at a certain position in the environment.
It might seem similar to normal stereo sound, but the big difference is
that you can turn around and face any direction. This means that the
sound from each speaker is adjusted for the direction you are facing.

The capabilities mentioned can be leveraged by HoloLens applications to
produce unprecedented experiences and possibilities for interactions.

## Combining Unity and Visual Studio

You can program applications for the HoloLens in a variety of ways. The
applications can be any standard Universal application for Windows 10.
They are 2-dimensional windowed applications that you place somewhere in
the environment on a flat surface like a wall or a table surface.

More immersive applications use the capabilities of the HoloLens to
enhance beyond mere projection into the environment. Such applications
use a low level C++ programming model using DirectX, or a higher level
managed language (such as C#) combined with the .NET framework.

During the Holographic Academy we developed a Origami demo, where a
playground of paper origami objects was placed into the classroom. The
playground consisted of a notepad of paper sheets, two folded paper
planes and two balls. We added functionality and logic to drop the balls
onto the planes and let them roll down into our real world. These
actions are initiated both by voice and gesture commands. Once dropped
the balls actually bounced against holographic, but also real world
objects recognized by the HoloLens from the spatial map information. In
the final exercise we changed the logic such that when the ball dropped
to the ground, it show an explosion on the point of impact. After the
explosion a virtual hole in the ground was shown. Through this hole
there appeared to be a complete new 3D world underneath our classroom
floor. Watching into the hole let you view that world from all angles as
if it was really there.

We used Unity 5.0 with some changes and extensions for the HoloLens
device. Unity is mainly used for 3D applications and games. It
simplifies the use of 3D objects, grids and meshes and has a physics
engine to mimic real world interactions between objects and the world.
Together with the extensions Unity is a perfect fit as it has all the
ingredients to mix real world characteristics and the input and output
of the HoloLens sensors and lenses.

![](./media/image3.png)

Typically you create a scene with a camera in the Unity development
environment. The camera views the scene consisting of several objects
and the controls allow you to interact with the object and the camera's
position and direction. HoloLens brings the combination of the
environment and the scene, plus the interaction through the sensors
instead of keyboard and mouse. The addition of spatial mapping allows
the scene to interact with the actual layout of the room, because it
will give the physics engine the contours and dimensions of the outline
of the real world objects. Add some script logic to the Unity mix and
you can create unprecedented applications.

## Programming for the Microsoft HoloLens

When building a HoloLens application the first thing to do is to remove
the default camera in the hierarchy of objects. You are viewing the
world from the HoloLens and not from the traditional camera. The
Hololens SDK provides a special HoloLens "camera" that you can use in
the Unity environment. This is a special stereoscopic camera that is
positioned and oriented by yourself and you move around in the
environment, turn and look up and down.

The HoloLens SDK also provides several entities to be used in the Unity
programming environment. For example, it has a SpatialMapping object
that is added to the hierarchy of objects in the application. It will
automatically add the spatial layout obtained by the sensors and the HPU
to the scene. The Unity rendering and physics engine take care of the
processing.

Unity uses scripts to add behavior and functionality to the entities in
the application you are building. The script can be written in a either
C# or JavaScript. We created the scripts in C#, by simply clicking the
Scripts pane from Unity and adding the appropriate script. You couple
these scripts to the object or artefact in your Unity scene, thereby
attaching the functionality in the script to the entity.

A simple double-click on a script transitions from the Unity environment
to Visual Studio. The Visual Studio solution you created earlier
contains all the scripts you have added. It also holds any other C# code
that you need to use, such as helper classes or logic.
![](./media/image4.png)


In general code and logic inside Unity is related to entities. The key
events for entities is the startup and an update of the entity's state
for rendering another display frame.

Let's look at an example scenario of creating a behavior: cursor
placement. A HoloLens application can use a cursor to indicate the
direction of your gaze and to provide visual feedback of the actual
object or surface you are interacting with. You need a visual mesh to
render as the cursor and some logic to define the behavior. The visual
mesh here is a donut shape that we can then place on the place where we
look at, so it looks like we have a visual cursor following our gaze.

The code fragment below shows the skeleton of such a class with the
particular startup and update logic. The implementation is indicative of
the abstraction level of the SDK.

public class WorldCursor : MonoBehaviour {

    private MeshRenderer meshRenderer;

 

    // Use this for initialization

    void Start()

    {

        // Grab the mesh renderer that\'s on the same object as this
script.

        meshRenderer =
this.gameObject.GetComponentInChildren\<MeshRenderer\>();

    }

   

    // Update is called once per frame

    void Update()

    {

        // Do a raycast into the world based on the user\'s

        // head position and orientation.

        var head = StereoCamera.Instance.Head;

        RaycastHit hitInfo;

        if (Physics.Raycast(head.position, head.forward, out hitInfo))

        {

            // If the raycast hit a hologram\...

 

            // Display the cursor mesh.

            meshRenderer.enabled = true;

            // Move the cursor to the point where the raycast hit.

            this.transform.position = hitInfo.point;

            // Rotate the cursor to hug the surface of the hologram.

            this.transform.rotation =

                Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

        }

        else

        {

            // If the raycast did not hit a hologram, hide the cursor
mesh.

            meshRenderer.enabled = false;

        }

    }

}

The Start method typically does one-time initialization. In the code
example it will acquire the mesh of the donut we renderer for the
cursor. Inside the Update method you perform the following steps:

1.  Acquire the direction of the gaze as a vector in 3D space from the
    HoloLens.

2.  Perform a raycast in the direction of the gaze and test whether an
    actual object or surface was hit. Casting a ray is like shooting a
    laser beam straight from the origin, in this case front of the
    HoloLens. The hit test of the beam tells whether have focused on
    something or are just staring into an empty space. This raycast is
    available in Unity and not HoloLens specific.

3.  If there was a hit, determine the normal vector of the hit location.
    The normal is the vector perpendicular to the surface (of the
    object).

4.  Perform a transformation (rotation) of the cursor to match the
    direction of the normal.\
    This will make the cursor follow the curves of the object or
    surface, giving it a natural feel of clinging to the real world
    objects or holograms. This is done by a standard 3D operation called
    a Quaternion rotation.

5.  Display the cursor by rendering its mesh.

By adding the cursor mesh and attaching the logic from the WorldCursor
class we get the cursor behavior in just a couple of lines of code.

Once your application is built from both Unity and Visual Studio, you
can deploy it to the HoloLens device through the USB cable that connects
your development computer and the HoloLens. Running the application is
as simple as selecting Run without Debugging from the Debug menu in
Visual Studio. The HoloLens provides a web interface that you can use to
upload programs and start and stop them. You can just open a browser
window and navigate to the HoloLens' IP address, which will show you a
web page to interact with. On this page you can upload the standard
windows 10 Universal App packages. Once uploaded they can be run from
either this web interface or straight from Visual Studio.

## In conclusion

Programming and testing our first HoloLens application was an incredible
experience. We had no prior knowledge of Unity, but plenty of years
experience programming in C# and a variety of frameworks. The use of
Unity takes some getting used to. Fortunately there is an abundance of
online tutorials and documentation and an active community. With the
help of the mentor and the documentation we were able to create our
origami demo within 4 hours and have it running on the actual HoloLens
devices. This was a truely amazing and inspiring experience..

In the Windows 10 timeframe Microsoft will release the HoloLens device
and make it available to the general public. Presumably before that time
developers might get their hands on the device and SDK to start
developing compelling and innovative applications for the HoloLens.
There is an incredible amount of new possibilities available
applications for the HoloLens. We can't wait to get started for real and
put holograms to use. The question remains what the product release date
and initial costs will be..

Many thanks to Emily and the HoloLens product team for assisting at the
Holographic Academy and reviewing this article.
