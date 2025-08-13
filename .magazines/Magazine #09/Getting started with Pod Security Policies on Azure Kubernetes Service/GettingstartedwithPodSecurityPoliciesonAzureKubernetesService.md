By Loek Duys

# Intro

Azure Kubernetes Service or AKS, is a semi-managed container
orchestrator cluster, running Kubernetes. You can use it to run
different kinds of workloads, e.g. web servers and background workers.

Even though it's called a 'managed' cluster, as an AKS consumer you are
responsible for upgrading Kubernetes versions and rebooting nodes to
apply security patches. You are also responsible for application
security matters, such as running pods using the principle of 'least
privilege[^1]', which means that containers do not have any capabilities
they do not explicitly require to run.

If an attacker gains control over one of your pods, they can use it to
attack the rest of the system. In this article you will learn how
restricting pod privileges will make it **much** harder for an attacker
to use a compromised container to attack the rest of the system.

# Pod security policies

To run your containers with the least amount of privileges, you can use
a tool within Kubernetes that is called 'Pod Security Policy' or PSP. A
PSP defines what containerized processes can and cannot do. It works on
the pod level, so it applies to all containers running within the pod.
For example, a policy can be used to restrict network, disk, and access
to the container host kernel. Policies are defined at the cluster level
and can be applied to **all** starting pods automatically. This way, you
cannot accidentally forget to restrict pod settings when deploying new
software to your cluster.

When enabling the feature, which currently is in preview, on AKS, you
will get two (cluster-level) policies straight out of the box.

1.  privileged -- using this policy has the same effect as using no
    policy at all, **all** operations are permitted. This policy can be
    useful in test scenarios, but you should use it with care.

2.  restricted -- using this policy applies a set of restrictions. For
    example, it prevents the container from running as root. After
    enabling the feature, this is the default policy that gets applied
    to all new pods.

Of course, you can also define custom policies to match your security
requirements even more closely.

## What they do

Pod Security Policies restrict containerized processes in the following
aspects:

  -----------------------------------------------------------------------
  What                Prevent or allow...
  ------------------- ---------------------------------------------------
  Privilege           containers to run as *root*, or to escalate to
  restrictions        *root*. Use of privileged *fsgroup* and *group*

  Process             container process access to capabilities like
  capabilities        'CAP_NET_BIND_SERVICE,' that controls the use of a
                      privileged network port.

  Host access         containers to access processes, storage, and
                      network on the container host.

  Container root      processes to write to the root volume within the
  volume              container.

  Volumes             access to types of volumes attached to pods.

  SELinux, AppArmor,  the use of these Linux security features. For
  seccomp             example, you can use Seccomp to disallow a process
                      from making unsafe system calls. AppArmor is used
                      to restrict process capabilities. Note that the
                      AppArmor and SECComp features are currently in
                      preview.
  -----------------------------------------------------------------------

For more details, have a look at the documentation here[^2].

# How to get started?

Enabling the PSP feature applies the 'restricted' policy to all new
pods, which could potentially make your system unusable. So you should
always create and apply policies first and enable the feature second.
This way, you won't break running systems.

To make sure you don\'t block or break stuff in the 'kube-system'
namespace, every pod deployed in that namespace can be configured to run
using the built-in \`privileged\` policy, which allows all rights to
pods; privilege escalation, privileged ports, and read/write access to
the container root file system. You can also restrict privileges by
using a custom policy.

In the following paragraphs, I'll explain how you can configure your
containers to support running in restricted environments. I will do this
for two well-known platforms; Nginx and Kestrel.

## Prevent the pod from running root

By default, a container is allowed to run as root. Running a container
as root is risky because it allows complete access to everything within
the container. These privileges can be used by an attacker, to break out
of the container[^3] and access the container host. To run a container
as non-root, you must make sure it does not access resources that
require privileges. For example, it must not write to protected folders.

### Nginx 

If you are running Nginx as a web server, have a look at the
'nginxinc/nginx-unprivileged' image. It can run as non-root, does not
use privileged ports, and does not access privileged locations on disk.

### Kestrel

If you are running a dotnet core web application on Kestrel, make sure
to configure it to run on a port higher than #1024. For example, you can
do this by defining these environment variables in the Kubernetes
template:

ENV ASPNETCORE_URLS=\"https://+:8001\"

ENV ASPNETCORE_HTTPS_PORT=8001

EXPOSE 8001

Specify a security context in your pod definition to indicate the user
that runs the containers. You could define your deployment as shown in
Figure 1. The values of 'runAsUser' and 'runAsGroup' should be a number
above 999, all lower numbers are usually reserved by the system.

## Prevent the pod from writing to the file system

Denying a pod write access to its file system can prevent an attacker
from downloading and installing tools within a compromised container.
You will need to make sure that the container does not require write
access to function.

### Nginx

Running Nginx without write access is tricky because it buffers large
requests & responses and log files on disk. There are (un-supported)
ways to get it to work[^4], but I haven't had success running our
application.

### Kestrel

Running a dotnet core process on Kestrel can be done, but that also
requires an undocumented workaround. You need to disable a feature
called COMPlus diagnostics (which seems to be there for diagnostic
support) by defining an environment variable in your Kubernetes template
or dockerfile:

COMPlus_EnableDiagnostic=0

By applying these measures, you can now safely run such containers using
the 'restricted' PSP. But how do you create a policy and apply it to a
pod?

## Defining a policy

You can create a custom policy by deploying a YAML file to the
Kubernetes cluster. Your restrictive policy could look like Figure 2:

The first lines enable the use of seccomp and AppArmor (default)
profiles by using annotations. The policy also prevents running as root
and use of root groups, using a non-zero value. It also prevents access
to the host. Note that the last line denies pods access to the root file
system within the container.

## Applying a policy

You can apply a Pod security policy to a pod, by using 'Role-Based
Access Control' (RBAC). First, you create a ClusterRole that allows the
cluster-wide use of the policy, as you see in Figure 3:

You may have noticed that the name of the policy starts with '00'; this
is because policies are applied in alphabetical order when multiple
policies match the pod requirements. The built-in 'restricted' policy
applies to every authenticated user, so to apply your policy it must be
higher in the alphabetical sorting order. Adding the '00' prefix ensures
your policy prevails.

We now have a role that allows the use of the custom policy. The next
step is to configure a service account with that role. We can do this by
creating a new service account to run pods in a namespace, and a
RoleBinding that connects the service account to the cluster role, as
displayed in Figure 4:\
We configured the pod security context to use the service account named
'be-pods' using the setting 'serviceAccountName'. If we now run this
deployment, all new pods will use the PSP named '00-restricted-policy'.
Every pod that runs under this service account in the namespace 'prod'
will be **forced** to comply with the attached policy.

I've shown a very specific way to bind a specific service account to a
PSP. Note that you can use various settings in the 'subjects' property
to target multiple service accounts, for example, to include **all**
service accounts inside a namespace. Read more about this online[^5].

You may have noticed that we create the service account with an
additional setting 'automountServiceAccountToken' with the value
'false'. We do this to prevent Kubernetes from providing an API token to
access the management API to pods. Most containers don't need access to
management API. Omitting this token from pods is an additional security
measure.

## 

Figure : From PSP to Pod

Figure 5 shows a schematic flow that describes how a policy is applied
to a pod.

## Checking which policy is applied

You can use the 'kubectl' CLI tool to see the effect of a policy applied
to your pod. Examine the output of this command:

kubectl get pod/webapi-6cbd96c775-s42pq \--namespace prod -o yaml\
\
apiVersion: v1

kind: Pod

metadata:

annotations:

container.apparmor.security.beta.kubernetes.io/backend: runtime/default

kubernetes.io/psp: 00-restricted-policy

seccomp.security.alpha.kubernetes.io/pod: runtime/default

Please note that the annotation 'kubernetes.io/psp' indicates the value
'00-restricted-policy'. This value means that the custom PSP was applied
to this pod. If the pod reported the value 'restricted', it would mean
that the built-in 'restricted' default policy was applied instead.

## Checking service account role binding

You can verify that a service account is configured properly to use the
cluster role by examining the output of this command:

kubectl \--as=system:serviceaccount:prod:be-pods \`\
\--namespace prod auth can-i use podsecuritypolicy/00-restricted-policy

yes

If the output value is 'yes', the service account is allowed to use the
custom policy. You can also assert that the service account can **not**
use the built-in privileged policy:

kubectl \--as=system:serviceaccount:prod:be-pods \`\
\--namespace prod auth can-i use podsecuritypolicy/privileged

no

# Dealing with disaster

Once you have enabled the PSP feature with incorrectly configured
policies, your pods may fail to start. You may have configured too many
restrictions to your pods, or system services may be affected by the
built-in 'restricted' policy. If you cannot fix this immediately, you
can disable the feature by using the following CLI commands:

az account set \--subscription \<\<your subscription\>\>

az aks update \--resource-group \<\<group\>\> \--name \<\<cluster\>\>
\--disable-pod-security-policy

Disabling the feature will not remove any existing policies, roles or
bindings. The policies will simply not be enforced any longer.

To (re)enable the feature:

az account set \--subscription \<\<your subscription\>\>

az aks update \--resource-group \<\<group\>\> \--name \<\<cluster\>\>
\--enable-pod-security-policy

# Conclusion

Pod security policies provide a powerful tool that restricts privileges
assigned to your pods without you having to define rules for every
individual pod. If you are not already using them, start using Pod
Security Policies today, and make it much more difficult for compromised
containers to harm the rest of your system.

[^1]: <https://en.wikipedia.org/wiki/Principle_of_least_privilege>

[^2]: <https://kubernetes.io/docs/concepts/policy/pod-security-policy/#what-is-a-pod-security-policy>

[^3]: <https://blog.trailofbits.com/2019/07/19/understanding-docker-container-escapes/>

[^4]: <https://medium.com/urban-massage-product/nginx-with-docker-easier-said-than-done-d1b5815d00d0>

[^5]: <https://kubernetes.io/docs/reference/access-authn-authz/rbac/#rolebinding-and-clusterrolebinding>
