---
layout: "post"
title: "Securing Your MCP Server with JWT Authentication and Authorization"
description: "This guide, authored by wassimchegham, walks through securing a Node.js-based Model Context Protocol (MCP) server using JSON Web Tokens (JWT) for authentication and role-based access control. It covers implementation steps, code examples, middleware integration, and best practices for robust, scalable security tailored for LLM tool integrations and AI agents. Practical advice is provided for establishing user roles, permissions, and centralized JWT management, with reference to an Azure-hosted MCP project."
author: "wassimchegham"
excerpt_separator: <!--excerpt_end-->
canonical_url: "https://techcommunity.microsoft.com/t5/microsoft-developer-community/it-s-time-to-secure-your-mcp-servers-here-s-how/ba-p/4434308"
viewing_mode: "external"
feed_name: "Microsoft Tech Community"
feed_url: "https://techcommunity.microsoft.com/t5/s/gxcuf89792/rss/Category?category.id=Azure"
date: 2025-09-25 07:00:00 +00:00
permalink: "/community/2025-09-25-Securing-Your-MCP-Server-with-JWT-Authentication-and-Authorization.html"
categories: ["Azure", "Coding", "Security"]
tags: ["API Security", "Authentication", "Authorization", "Azure", "Azure Samples", "Coding", "Community", "Express.js", "JSON Web Tokens", "JWT", "LLM", "MCP", "Microservices", "Middleware", "Node.js", "RBAC", "Role Based Access Control", "Security", "Token Based Security"]
tags_normalized: ["api security", "authentication", "authorization", "azure", "azure samples", "coding", "community", "expressdotjs", "json web tokens", "jwt", "llm", "mcp", "microservices", "middleware", "nodedotjs", "rbac", "role based access control", "security", "token based security"]
---

wassimchegham provides a practical tutorial on securing Node.js-based MCP servers using JWT authentication and authorization, emphasizing scalable, role-based security controls for AI-enabled architectures.<!--excerpt_end-->

# Securing Your MCP Server with JWT Authentication and Authorization

Author: **wassimchegham**

The Model Context Protocol (MCP) is a standard for enabling large language models (LLMs) to interact with external tools. However, once you move beyond local prototypes and expose your MCP server to real users and agents, robust security becomes non-negotiable. This guide details a practical approach for armoring your Node.js MCP server using JSON Web Tokens (JWT) and role-based access control (RBAC).

## Why Secure Your MCP Server?

Leaving an MCP server unprotected is equivalent to leaving your infrastructure vulnerable to unauthorized tool usage, data leaks, or outright disruption. With LLM agents in play, every endpoint must be protected from both accidental and malicious access.

## The Security Stack: Goals and Approach

1. **Authentication**: Confirming the identity of the requester on every API call.
2. **Authorization**: Assigning fine-grained permissions based on the authenticated user's role (admin, user, readonly, etc).
3. **Tool Protection**: Ensuring only authorized individuals or agents access designated tools or APIs.

## Why JWT?

JWTs are especially well-suited for securing MCP APIs:

- **Stateless** – Authentication data is embedded within the token, ideal for scaling AI agent traffic.
- **Self-Contained** – User roles and permissions travel with each request.
- **Tamper-Proof** – Tokens are cryptographically signed.
- **Microservices Ready** – Same JWT can span multiple APIs/services.

## Implementation Breakdown

### 1. Defining Roles and Permissions

Lay out a clear permission model:

```typescript
export enum UserRole {
  ADMIN = "admin",
  USER = "user",
  READONLY = "readonly",
}

export enum Permission {
  CREATE_TODOS = "create:todos",
  READ_TODOS = "read:todos",
  UPDATE_TODOS = "update:todos",
  DELETE_TODOS = "delete:todos",
  LIST_TOOLS = "list:tools",
}

export interface AuthenticatedUser {
  id: string;
  role: UserRole;
  permissions: Permission[];
}

const rolePermissions: Record<UserRole, Permission[]> = {
  [UserRole.ADMIN]: Object.values(Permission),
  [UserRole.USER]: [
    Permission.CREATE_TODOS,
    Permission.READ_TODOS,
    Permission.UPDATE_TODOS,
    Permission.LIST_TOOLS,
  ],
  [UserRole.READONLY]: [
    Permission.READ_TODOS,
    Permission.LIST_TOOLS,
  ],
};
```

### 2. Centralized JWT Service

Create a dedicated module for token creation and verification, using environment variables to protect secrets.

```typescript
import * as jwt from "jsonwebtoken";
// ... (imports from authorization module)

const JWT_SECRET = process.env.JWT_SECRET!;
const JWT_AUDIENCE = process.env.JWT_AUDIENCE!;
const JWT_ISSUER = process.env.JWT_ISSUER!;
const JWT_EXPIRY = process.env.JWT_EXPIRY || "2h";

export function generateToken(user: Partial<AuthenticatedUser> & { id: string }): string {
  const payload = {
    id: user.id,
    role: user.role || UserRole.USER,
    permissions: user.permissions || getPermissionsForRole(user.role || UserRole.USER),
  };
  return jwt.sign(payload, JWT_SECRET, {
    algorithm: "HS256",
    expiresIn: JWT_EXPIRY,
    audience: JWT_AUDIENCE,
    issuer: JWT_ISSUER,
  });
}

export function verifyToken(token: string): AuthenticatedUser {
  // ...verify logic, throw on invalid or expired tokens
}
```

### 3. Authentication Middleware

Protect every endpoint by injecting middleware that verifies the JWT and populates the request context with the user details.

```typescript
import { Request, Response, NextFunction } from "express";
import { verifyToken } from "./auth/jwt.js";

declare global {
  namespace Express {
    interface Request {
      user?: AuthenticatedUser;
    }
  }
}

export function authenticateJWT(req: Request, res: Response, next: NextFunction): void {
  const authHeader = req.headers.authorization;
  if (!authHeader || !authHeader.startsWith("Bearer ")) {
    return res.status(401).json({ error: "Authentication required" });
  }
  const token = authHeader.substring(7);
  try {
    req.user = verifyToken(token);
    next();
  } catch (error) {
    res.status(401).json({ error: "Invalid token", message: error.message });
  }
}
```

### 4. Securing Endpoints and Tool Access

Attach the `authenticateJWT` middleware only to protected endpoints, and add granular permission checks before serving sensitive data or operations (e.g., listing tools, executing actions).

```typescript
// Within your server class or router
app.use("/mcp", authenticateJWT);

// Example RBAC check inside a handler
if (!hasPermission(user, Permission.LIST_TOOLS)) {
  return res.status(403).json({ error: "Insufficient permissions" });
}
```

### 5. MCP Specification Compliance

This solution offers strong security with JWT and RBAC. Note, however, it does not (yet) fully implement the [MCP authorization specification](https://modelcontextprotocol.io/specification/2025-06-18/basic/authorization). Additional steps may be needed for strict spec compliance—this can be incrementally built atop the approach covered here.

## Summary and Next Steps

By following this workflow, you have:

- Defined explicit roles and permission sets
- Centralized JWT logic for issuing and verifying tokens
- Built an authentication middleware for the Express server
- Enforced permission checks to protect API endpoints and internal resources

Continue enhancing your authorization logic according to future requirements, and consider subscribing to updates on the [Azure-Samples/mcp-container-ts GitHub repo](https://github.com/Azure-Samples/mcp-container-ts).

This post appeared first on "Microsoft Tech Community". [Read the entire article here](https://techcommunity.microsoft.com/t5/microsoft-developer-community/it-s-time-to-secure-your-mcp-servers-here-s-how/ba-p/4434308)
