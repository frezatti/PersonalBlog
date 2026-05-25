# PersonalBlog - Backend API

API backend para um **Blog Pessoal**, desenvolvida em **ASP.NET Core Web API**, com **Entity Framework Core**, **PostgreSQL**, autenticação via **JWT**, criptografia de senhas com **PasswordHasher**, arquitetura em camadas e controle de acesso por perfil de usuário.

Este projeto foi desenvolvido como atividade acadêmica da disciplina/projeto de backend em .NET, seguindo a proposta de criação de uma API REST para gerenciamento de usuários, temas e postagens.

---

## Objetivo do projeto

O objetivo da aplicação é fornecer uma API para um blog pessoal, permitindo:

- cadastro e autenticação de usuários;
- criação, consulta, atualização e remoção de temas;
- criação, consulta, atualização e remoção de postagens;
- filtragem de postagens por autor e/ou tema;
- proteção de rotas por autenticação JWT;
- autorização baseada no tipo de usuário, diferenciando usuários comuns e administradores.

---

## Tecnologias utilizadas

- **C#**
- **.NET 10**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT Bearer Authentication**
- **PasswordHasher** para armazenamento seguro de senhas
- **Swagger/OpenAPI** para documentação e testes manuais da API
- **xUnit** para testes automatizados

---

## Arquitetura do projeto

O projeto segue uma arquitetura em camadas, separando responsabilidades entre controllers, services, repositories e acesso ao banco de dados.

Fluxo geral:

```text
Controller -> Service -> Repository -> AppDBContext -> Banco de Dados
```

### Camadas principais

```text
PersonalBlog/
├── Controllers/      # Endpoints da API
├── Data/             # DbContext da aplicação
├── DTOs/             # Objetos de entrada e saída da API
├── Models/           # Entidades do domínio
├── Repositories/     # Acesso aos dados
├── Services/         # Regras de negócio
├── Migrations/       # Migrações do Entity Framework Core
└── Program.cs        # Configuração da aplicação
```

### Entidades principais

- **User**: representa os usuários da aplicação.
- **Topic**: representa os temas/categorias das postagens.
- **Post**: representa as postagens do blog.

### Perfis de usuário

O projeto utiliza um enum para representar o tipo do usuário:

```csharp
public enum UserType
{
    User = 0,
    Admin = 1
}
```

O perfil do usuário é incluído no token JWT como uma role. Assim, endpoints administrativos podem ser protegidos com:

```csharp
[Authorize(Roles = "Admin")]
```

---

## Segurança

A aplicação implementa autenticação e autorização com JWT.

### Senhas

As senhas dos usuários **não são armazenadas em texto puro**. O projeto utiliza `PasswordHasher<User>` para gerar e verificar o hash das senhas.

### JWT

Ao realizar login, a API gera um token JWT contendo informações do usuário, incluindo seu identificador, e-mail e perfil de acesso.

O token deve ser enviado nas rotas protegidas usando o cabeçalho:

```http
Authorization: Bearer SEU_TOKEN_AQUI
```

### Controle de acesso

Exemplos de regras de acesso:

- Cadastro e login são públicos.
- Consulta de temas e postagens é pública.
- Criação, atualização e remoção de postagens exigem autenticação.
- Gerenciamento de usuários e temas exige perfil de administrador.

---

## Endpoints principais

A URL base local padrão é:

```text
http://localhost:5229
```

Também existe perfil HTTPS local:

```text
https://localhost:7272
```

### Autenticação e usuários

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `POST` | `/api/usuarios` | Público | Cadastra um novo usuário |
| `POST` | `/api/usuarios/cadastrar` | Público | Cadastra um novo usuário, rota compatível com o enunciado |
| `POST` | `/api/usuarios/login` | Público | Autentica o usuário e retorna um token JWT |
| `GET` | `/api/usuarios` | Admin | Lista todos os usuários |
| `GET` | `/api/usuarios/{id}` | Admin | Busca um usuário por ID |
| `PUT` | `/api/usuarios/{id}` | Admin | Atualiza um usuário |
| `DELETE` | `/api/usuarios/{id}` | Admin | Remove um usuário |

Exemplo de cadastro:

```json
{
  "name": "Adriel Frezatti",
  "email": "adriel@example.com",
  "password": "12345678"
}
```

Exemplo de login:

```json
{
  "email": "adriel@example.com",
  "password": "12345678"
}
```

---

### Temas

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/temas` | Público | Lista todos os temas |
| `GET` | `/api/temas/{id}` | Público | Busca um tema por ID |
| `POST` | `/api/temas` | Admin | Cria um tema |
| `PUT` | `/api/temas/{id}` | Admin | Atualiza um tema |
| `DELETE` | `/api/temas/{id}` | Admin | Remove um tema |

Exemplo de criação de tema:

```json
{
  "description": "Tecnologia"
}
```

---

### Postagens

| Método | Rota | Acesso | Descrição |
|---|---|---|---|
| `GET` | `/api/postagens` | Público | Lista todas as postagens |
| `GET` | `/api/postagens/{id}` | Público | Busca uma postagem por ID |
| `GET` | `/api/postagens?userId={id}&topicId={id}` | Público | Filtra postagens por usuário e/ou tema |
| `GET` | `/api/postagens/filtro?autor={id}&tema={id}` | Público | Filtra postagens usando os nomes de parâmetros do enunciado |
| `POST` | `/api/postagens` | Autenticado | Cria uma postagem |
| `PUT` | `/api/postagens/{id}` | Autenticado | Atualiza uma postagem |
| `DELETE` | `/api/postagens/{id}` | Autenticado | Remove uma postagem |

Exemplo de criação de postagem:

```json
{
  "title": "Minha primeira postagem",
  "content": "Conteúdo da postagem.",
  "topicId": 1,
  "userId": 1
}
```

Exemplo de atualização de postagem:

```json
{
  "title": "Postagem atualizada",
  "content": "Novo conteúdo da postagem.",
  "topicId": 1
}
```

---

## Validações

O projeto utiliza validações em DTOs e também validações na camada de serviço.

Exemplos de validações aplicadas:

- nome de usuário obrigatório;
- e-mail obrigatório e em formato válido;
- senha obrigatória com tamanho mínimo;
- título de postagem obrigatório e com limite de caracteres;
- conteúdo da postagem obrigatório;
- identificadores de usuário e tema devem ser maiores que zero.

Como os controllers utilizam `[ApiController]`, entradas inválidas podem retornar automaticamente `400 Bad Request`.

---

## Configuração local

### 1. Pré-requisitos

Antes de executar o projeto, é necessário ter instalado:

- .NET SDK compatível com o projeto;
- PostgreSQL;
- ferramenta `dotnet-ef`, caso precise aplicar migrações;
- opcionalmente, Insomnia ou Postman para testes manuais.

Para instalar a ferramenta do Entity Framework, se necessário:

```bash
dotnet tool install --global dotnet-ef
```

---

### 2. Clonar o repositório

```bash
git clone https://github.com/frezatti/PersonalBlog.git
cd PersonalBlog
```

Restaurar dependências:

```bash
dotnet restore
```

---

### 3. Configurar banco de dados

O projeto utiliza PostgreSQL. Configure a connection string com **User Secrets**, evitando salvar senhas reais no repositório.

Execute o comando a partir da pasta que contém o arquivo `PersonalBlog.csproj`:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=personal_blog_db;Username=postgres;Password=SUA_SENHA"
```

Depois, aplique as migrações:

```bash
dotnet ef database update
```

---

### 4. Configurar JWT

Gere uma chave local para o JWT:

```bash
openssl rand -base64 32
```

Configure a chave usando User Secrets:

```bash
dotnet user-secrets set "Jwt:Key" "COLE_A_CHAVE_GERADA_AQUI"
dotnet user-secrets set "Jwt:Issuer" "PersonalBlog"
dotnet user-secrets set "Jwt:Audience" "PersonalBlog"
```

A chave real **não deve ser enviada para o GitHub**.

---

### 5. Executar o projeto

A partir da raiz do repositório:

```bash
dotnet run --project PersonalBlog
```

Ou, dentro da pasta do projeto:

```bash
dotnet run
```

A API estará disponível, por padrão, em:

```text
http://localhost:5229
```

Swagger em ambiente de desenvolvimento:

```text
http://localhost:5229/swagger
```

---

## Como testar a API manualmente

### Fluxo recomendado

1. Criar um usuário em `POST /api/usuarios/cadastrar`.
2. Promover esse usuário para administrador no banco de dados, caso queira testar rotas administrativas.
3. Fazer login em `POST /api/usuarios/login`.
4. Copiar o token retornado.
5. Enviar o token nas rotas protegidas com `Authorization: Bearer TOKEN`.
6. Criar temas.
7. Criar postagens.
8. Testar listagens, filtros, atualizações e remoções.

### Promover usuário para administrador

Como o cadastro padrão cria usuários comuns, para testar rotas administrativas é possível promover manualmente um usuário no PostgreSQL:

```sql
UPDATE "User"
SET "Type" = 1
WHERE "Email" = 'adriel@example.com';
```

Depois disso, faça login novamente para obter um token atualizado com role de administrador.

---

## Testes automatizados

O repositório possui um projeto de testes chamado `PersonalBlog.Tests`.

Para executar os testes:

```bash
dotnet test
```

Esse comando compila a solução e executa os testes existentes.

---

## Build do projeto

Para verificar se a aplicação compila corretamente:

```bash
dotnet build
```

---

## Migrações do Entity Framework Core

Para criar uma nova migração:

```bash
dotnet ef migrations add NomeDaMigracao --project PersonalBlog
```

Para aplicar migrações ao banco:

```bash
dotnet ef database update --project PersonalBlog
```

---

## Observações sobre campos de IA

A entidade `Post` contém campos preparados para funcionalidades futuras relacionadas a IA, como resumo, tags e categoria gerada automaticamente:

- `AiSummary`
- `AiTags`
- `AiCategory`

Esses campos estão disponíveis no modelo para evolução futura do projeto.

---

## Qualidade e boas práticas aplicadas

O projeto aplica boas práticas como:

- separação em camadas;
- uso de DTOs para entrada e saída de dados;
- uso de repositories para acesso ao banco;
- uso de services para regras de negócio;
- autenticação JWT;
- autorização por roles;
- hashing de senhas;
- uso de migrations;
- documentação interativa com Swagger;
- validação de entrada.

---

## Autor

Projeto desenvolvido por **Adriel Frezatti** como parte de atividade acadêmica de backend com .NET.
