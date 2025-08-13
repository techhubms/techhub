*By Chris van Sluijsveld and Loek Duys*

You have probably heard of Blazor and that it uses WebAssembly to run
.NET code inside a browser. But did you know you can use WebAssembly for
much more? In this article, we will show you a few cool things to do
with WebAssembly.

# What is WebAssembly?

WebAssembly was invented as a language to run binary code inside a web
browser. Applications running in WebAssembly run isolated, just like
Docker containers. The use of virtualization allows a WebAssembly
program to be portable across operating systems and different processor
without modification. It runs on Windows, Mac, Linux, and devices like
the Raspberry Pi equally well. This is a big difference from containers,
which are created for specific operating systems and processor types.

WebAssembly, or \'Wasm,\' was invented by Mozilla and is now pushed
forward by the ByteCode Alliance, a group of companies including
Microsoft, Intel, and Google. Currently, Chrome, Edge, Safari, and
Firefox support running WebAssembly. Because it\'s a compact binary
format, it runs with little overhead at near-native speed. When compared
with JavaScript, WebAssembly applications usually run much faster. Many
popular programming languages can be compiled into WebAssembly and run
on the web. Supported languages include Rust, Python, Go and C. You can
even run the .NET Mono CLR in WebAssembly and use it to run regular .NET
DLLs. Blazor currently works like that. Microsoft is also experimenting
with a .NET runtime that compiles C# to Wasm.

Mozilla designed WebAssembly to co-exist with JavaScript and work
together to deliver a good web experience. A file containing WebAssembly
code is called a Module. An instance of a Module runs inside a sandboxed
execution environment. Sandboxing ensures WebAssembly cannot access
sensitive data like files and network resources without explicit consent
from the hosting environment, which is usually your web browser.
WebAssembly does that by importing and exporting functions from and to
its host.

## Beyond the browser

WebAssembly does not make any assumptions about the host environment it
runs on. This means that WebAssembly can also run outside of a browser.
The \'WebAssembly System Interface\' or \'WASI\' enables this. WASI is
an API that provides WebAssembly with operating-system functionality
like access to the file system and communication over networks. This
works using the function imports mentioned earlier. Most modern browsers
implement the WASI interface, and a few stand-alone implementations are
also available. Having a stand-alone WASI runtime means that a browser
is no longer required to run WebAssembly code. As with any good OSS
technology, there are quite a few WASI runtimes to choose from:
wasmtime, WAMR, Wasmer, WasmEdgeRuntime, Wasm3 and others. For this
article, we selected a host called wasmtime.

# Developing with WebAssembly

By now, you\'re probably anxious to get started running some WebAssembly
code. We will start by building and running a simple \'hello world\'
WebAssembly program inside a browser, using the Rust programming
language on Linux. Please note that if you are running Windows, you can
use WSL2 to follow the steps in this article.

## Running WebAssembly in your browser

We will create some Rust code and compile it into WebAssembly. If you
don't have them installed yet, please run this command to install the
Rust tools:

curl \--proto \'=https\' \--tlsv1.2 -sSf https://sh.rustup.rs \| sh

We want to create an interactive program that displays a message box to
the end user when executed. To do this inside a browser, we will import
JavaScript functions. To do this, we need a way to interact from
WebAssembly to JavaScript. We can use *wasm-pack* to do this. Run this
command to install wasm-pack:

curl https://rustwasm.github.io/wasm-pack/installer/init.sh -sSf \| sh

Now, let\'s create a simple Hello-World library in Rust by running:

cargo new \--lib hello-wasm

Change the directory to the newly created program folder:

cd hello-wasm/

The 'hello-wasm' folder contains a file named *Cargo.toml* that
describes how to build the project and a folder '*src*' with file named
*lib.rs* which we will change now. Modify the file to create a program
that outputs \'*Hello world!*\' when run. Use your favorite text editor
to change the code as displayed in Figure 1:

The first line imports an existing library named \'prelude\', so we can
use the features in the code following the \'use\' statement. The
*wasm-pack* tool uses the attribute '#\[wasm_bindgen\]' which enables
Rust to invoke JavaScript. The function \'alert\' exists in JavaScript
and is exposed to Rust by using the \'extern\' keyword. The other way
around works as well; the final section of the code exposes a Rust
method to JavaScript by using the \'pub\' keyword on the function named
\'greet\'. After compilation, we will have a WebAssembly program that
exposes a function 'greet. We will use JavaScript on a web page to
invoke that method. When executed, it will call the JavaScript 'alert'
function to create a message box.

Finally, change the *Cargo.toml* file to include the dependency to the
*wasm-pack* tool with the content from Figure 2.

We are now ready to run *wasm-pack* to compile the project using this
command:

wasm-pack build \--target web

After about half a minute, you should see output similar to this:

\[INFO\]: :-) Done in 27.08s

\[INFO\]: :-) Your wasm pkg is ready to publish at
/home/user/xpirit/magazine/rust/hello-wasm/pkg.

If you look at the files in that output directory, you should see a file
named \'hello_wasm.js\' and one called \'hello_wasm_bg.wasm\'. The wasm
file contains the Rust code, compiled into WebAssembly. The JavaScript
file contains everything needed to load your WebAssembly file into the
browser as a Module. Let\'s run the JavaScript file inside an HTML file
to show it works. Create a file named \'*index.html*\' inside the *pkg*
folder, with the content of Figure 3

As you can see, the web page uses JavaScript to call the 'greet' method
in WebAssembly. To see how that works, inspect the file
\'hello_wasm.js\'.

You will need a webserver to run this page and its JavaScript. The
script won\'t run if you open the local file directly from disk. If you
are using VS Code, you can use the \'Live Server\' extension by Ritwick
Dey. When it runs, you should see something similar to the screen from
Figure 4.

![Graphical user interface, text, application Description automatically
generated](./media/image1.png)


Figure 4: Hello World in browser

Congratulations! You have just built your first WebAssembly module,
hosted inside the browser!

## Running WebAssembly without a browser

WebAssembly can also be run locally on your machine, using a stand-alone
runtime. This is because WebAssembly relies on the WebAssembly System
Interface (WASI) to talk to the underlying operating system. The
WebAssembly System Interface does not depends on browsers and does not
have a requirement for JavaScript to run.

So for the next sample we will not use Javascript anymore but create a
console application that will run directly on your operating system
using WASI.

We will compile the Rust code in this example to WASI compliant
WebAssembly. We will replace the JavaScript calls with console input and
output, using the *stdin* and *stdout* streams. Earlier, we installed
Rust, this doesn\'t install the tools needed to compile to WASI
compliant WebAssembly. For that, we will need a new library named
'wasm32-wasi'. Install it using this command:

rustup target add wasm32-wasi

Now that you have installed the correct build target, we will create a
new project. You can do that by running the following command:

cargo new hellowasi

Change the directory to the newly created program folder:

cd hellowasi/

Next, compile the program to WASI compliant WebAssembly, by running:

cargo build \--release \--target wasm32-wasi

To run the compiled WebAssembly, we will need a stand-alone host. We
will use *wasmtime* for that. Install *wasmtime* using:

curl https://wasmtime.dev/install.sh -sSf \| bash

Run the *hellowasi* program with:

wasmtime ./target/wasm32-wasi/release/hellowasi.wasm

The output should say \"*Hello, World!*\".

Congratulations! You have just built and run your first native
WebAssembly program.

## Running WebAssembly with WAGI

We now have a nice console program writing its output to the *stdout*
stream. But what if you want to develop APIs in WebAssembly and host
them on any platform? This is where the current experimental WebAssembly
Gateway Interface (WAGI) comes in. To quote the WAGI website, **\"WAGI
allows you to run WebAssembly WASI binaries as HTTP handlers.\"**

Similar to how *wasm-pack* acts as a bridge between JavaScript and
WebAssembly, WAGI connects HTTP requests and responses to your
program\'s *stdin* and *stdout* streams.

To get started with WAGI, download the latest release from
<https://github.com/deislabs/wagi/releases> and unpack it. Next, we will
need to add a configuration file to tell WAGI where the WebAssembly
program we want to run is located for each requested URL. In our
example, we will use the root path at \"/\".

Create a wagi.toml file containing the content from Figure 5.

Because WAGI acts as an HTTP server, we need to make sure to write
required content-type headers to the output streams in our WebAssembly
program. Make sure your main.rs file looks like Figure 6.

Recompile the program using:

cargo build \--release \--target wasm32-wasi

After that, you should be able to WAGI to run your WebAssembly program
with the following command:

wagi -c wagi.toml

When WAGI is running, you can visit <http://localhost:3000> to see the
output. The result should look similar to Figure 7.

![Graphical user interface, text, application, chat or text message
Description automatically generated](./media/image2.png)


Figure 7: Hello World WAGI API in browser

Congratulations! You have just built and run your first WebAssembly API
server using WAGI.

# Moving to the Cloud

Running code in a light-weight isolated sandbox may seem familiar to
you. The way that code is executed in WebAssembly bears a lot of
resemblance with the way we run code inside containers. So, what if we
want to run our WebAssembly API at scale in the Cloud? We can do this
using Azure Kubernetes Service and an experimental service called
Krustlet, developed by Deis Labs. Krustlet acts as a Kubernetes node
that allows you to run WebAssembly programs on Kubernetes. Similar to
how Kubernetes nodes run containers based on images stored in an OCI
registry, Krustlet runs WebAssembly programs based on OCI artifacts (in
our case the wasm file).

OCI artifacts support many different formats. Aside from container
images, you can also store your WebAssembly program as OCI artifacts in
a suitable registry (for example, Azure Container Registry). To do this,
you will need another program called wasm-to-oci.

Download the latest release from
<https://github.com/engineerd/wasm-to-oci/releases> and then install it
using the following commands:

mv linux-amd64-wasm-to-oci wasm-to-oci

chmod +x wasm-to-oci

sudo cp wasm-to-oci /usr/local/bin

Make sure to have Azure Container Registry (ACR) available to test
things on and enable the 'anonymous pull' feature to allow WAGI to
access it without logging in.

Login to ACR and push your WebAssembly program to ACR with:

az acr login \--name acrwasmwasi

az acr update \--name acrwasmwasi \--anonymous-pull-enabled true

wasm-to-oci push ./target/wasm32-wasi/release/hellowasi.wasm
acrwasmwasi.azurecr.io/hellowasi:v1

Note: make sure to replace *acrwasmwasi* with the name of your container
registry.

Out of the box, WAGI, which we used earlier in the previous paragraph,
supports OCI artifacts as a source for WebAssembly code.

You can try this by updating the wagi.toml file created earlier to look
like Figure 8.

The run WAGI again using this command:

wagi -c wagi.toml

If this is successful, the output in the browser on
<http://localhost:3000> should look the same as it did in Figure 7.

## Running WebAssembly on AKS

Because Deis Labs is part of Microsoft, preview support for Krustlet
nodes is already available in Azure. If you want to learn more and try
it for yourself, follow the walkthrough on
<https://docs.microsoft.com/en-us/azure/aks/use-wasi-node-pools>

# Conclusion

You have seen a few interesting applications of WebAssembly. It could be
considered a highly secure way of running code in an isolated sandbox
locally, in the browser, in Kubernetes etc.

But there are some things you need to be aware of...

WebAssembly code in a binary format is hard to read. It\'s difficult to
know what code in a wasm file does from the outside, especially when it
is also obfuscated. Combine this with high execution speed and browser
support, and it becomes easy to see how WebAssembly can be used for
naughty things like running crypto-miners inside an unwitting user's web
browser. At the time of writing, there are no security scanning
platforms to scan images for malware and vulnerabilities.

Most, or all, of the current hosts are vulnerable to attacks like
SPECTRE, so at this time the examples shown here should not be used in a
production environment.

In the future, if these security risks are mitigated, WebAssembly could
become the next big thing after containers.

# Relevant links

  --------------------------------------------------------------------------------------------
  Link                                                               What
  ------------------------------------------------------------------ -------------------------
  https://hacks.mozilla.org/2018/10/webassemblys-post-mvp-future/    Describes the current
                                                                     state and future plans
                                                                     for WebAssembly

  https://bytecodealliance.org/                                      Website of the ByteCode
                                                                     Alliance.

  https://github.com/bytecodealliance/wasmtime-dotnet                Wasmtime for .NET

  https://deislabs.io/                                               Website of Deis Labs part
                                                                     of Microsoft.

  https://github.com/dotnet/runtimelab/tree/feature/NativeAOT-LLVM   Microsoft experimental
                                                                     runtime that compiles C#
                                                                     to wasm.
  --------------------------------------------------------------------------------------------

**Quote to put somewhere inside a square box:**

> If WASM+WASI existed in 2008, we wouldn\'t have needed to created
> Docker. That\'s how important it is. Webassembly on the server is the
> future of computing. A standardized system interface was the missing
> link. Let\'s hope WASI is up to the task!
>
> Solomon Hyke, Co-founder of Docker

**Another quote to put somewhere inside a square box:**

> Did you know Deis Labs used to be Deis which are the founders of Helm
> for Kubernetes. It became Deis Labs after the acquisition by Microsoft
> in April 2017. Deis Labs currently runs many experimental programs
> revolving around WebAssembly and Kubernetes.
