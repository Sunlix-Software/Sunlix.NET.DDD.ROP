# Sunlix.NET.DDD.ROP

[![.NET](https://img.shields.io/badge/.NET-6.0_|_8.0_|_9.0-blue)](https://dotnet.microsoft.com/)
[![NuGet](https://img.shields.io/nuget/v/Sunlix.NET.DDD.ROP.svg)](https://www.nuget.org/packages/Sunlix.NET.DDD.ROP/)
[![License](https://img.shields.io/github/license/Sunlix-Software/Sunlix.NET.DDD.ROP.svg)](LICENSE)

**Sunlix.NET.DDD.ROP** is a lightweight implementation of the *Railway-Oriented Programming* pattern for C#.  
It provides a composable `Result<TSuccess, TFailure>` type and a set of functional operators for building clear and predictable execution pipelines without exceptions.

---

## Table of Contents

- [Why this library](#why-this-library)
- [Installation](#installation)
- [Usage](#usage)
  - [Result](#result)
  - [Map / MapAsync](#map--mapasync)
  - [Bind / BindAsync](#bind--bindasync)
  - [Tee / TeeAsync](#tee--teeasync)
  - [TeeError / TeeErrorAsync](#teeerror--teeerrorasync)
- [FAQ](#faq)

---

## Why this library

* **Explicit error handling** — no hidden control flow via exceptions  
* **Composable pipelines** — chain operations in a predictable way  
* **Sync + async symmetry** — every operator has async equivalent  
* **Minimal abstraction** — no frameworks, just primitives
---

## Installation

```sh
dotnet add package Sunlix.NET.DDD.ROP
```

---

## Usage

This section contains minimal examples demonstrating the API.  
All examples are intentionally simplified and focus only on behavior.

---

## Result

```csharp
Result<User, Error> result = Result.Succeed<User, Error>(user);

if (result.IsSuccess)
{
    var value = result.Value;
}
```

---

## Map / MapAsync

Transforms success value.

```csharp
Result<User, Error> GetUser(int id) { ... }

UserDetails MapToDetails(User user) { ... }

Result<UserDetails, Error> result = GetUser(42)
    .Map(MapToDetails);
```

Async:

```csharp
async Task<UserDetails> MapToDetailsAsync(User user) { ... }

Result<UserDetails, Error> result = await GetUser(42)
    .MapAsync(MapToDetailsAsync);
```

---

## Bind / BindAsync

Chains operations that return `Result`.

```csharp
Result<User, Error> GetUser(int id) { ... }

Result<Subscription, Error> GetSubscription(User user) { ... }

Result<Subscription, Error> result = GetUser(42)
    .Bind(GetSubscription);
```

Async:

```csharp
async Task<Result<Subscription, Error>> GetSubscriptionAsync(User user) { ... }

Result<Subscription, Error> result = await GetUser(42)
    .BindAsync(GetSubscriptionAsync);
```

---

## Tee / TeeAsync

Executes side effects on success.

```csharp
void LogUser(User user) { ... }

Result<User, Error> result = GetUser(42)
    .Tee(LogUser);
```

Async:

```csharp
async Task LogUserAsync(User user) { ... }

Result<User, Error> result = await GetUserAsync(42)
    .TeeAsync(LogUserAsync);
```

---

## TeeError / TeeErrorAsync

Executes side effects on failure.

```csharp
void LogError(Error error) { ... }

Result<User, Error> result = await GetUserAsync(42)
    .TeeError(LogError);
```

Async:

```csharp
async Task LogErrorAsync(Error error) { ... }

Result<User, Error> result = await GetUserAsync(42)
    .TeeErrorAsync(LogErrorAsync);
```

---

## FAQ

<details>
<summary>Why not use exceptions?</summary>

Exceptions are for unexpected failures.  
This library models expected outcomes explicitly via `Result`.

</details>

<details>
<summary>When to use Map vs Bind?</summary>

- Use **Map** when transforming a value  
- Use **Bind** when the function returns another `Result`

</details>

<details>
<summary>Can I mix sync and async?</summary>

Yes. Every operation has async equivalents, and they compose naturally.

</details>
