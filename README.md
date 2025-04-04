# 📈 Calculadora Sinqia

API REST em .NET 8 que calcula o saldo atualizado de uma aplicação financeira com indexador pós-fixado, considerando a fórmula de rentabilidade composta (252 dias úteis).

---

## 🚀 Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core
- SQLite
- xUnit (Testes de Unidade)
- InMemoryDatabase (Testes)

---

## ⚙️ Como Executar o Projeto

1. Clone o repositório:

```bash
git clone https://github.com/ThomasFerraz/CalculadoraSinqia.git
cd CalculadoraSinqia
```

2. Restaure os pacotes NuGet:

```bash
dotnet restore
```

3. Atualize o banco de dados (SQLite):

```bash
dotnet ef database update
```

4. Execute a aplicação:

```bash
dotnet run
```

5. Acesse o Swagger para testar:

```
https://localhost:5001/swagger
```
(ou porta que o Visual Studio configurar)

---

## 📥 Estrutura da API

### Endpoint

- **POST** `/calculadora/calcular`

### Body JSON Exemplo

```json
{
  "valorInicial": 10000,
  "dataAplicacao": "2025-03-13",
  "dataFinal": "2025-03-21"
}
```

### Retorno Exemplo

```json
{
  "fatorJuros": 1.00274063296722,
  "valorFinal": 10027.40632967
}
```

---

## 🧠 Lógica de Cálculo

- O primeiro dia da aplicação **não gera rentabilidade** (rentabilidade começa no dia seguinte).
- Considera **apenas dias úteis** (exclui sábados e domingos).
- Para cada dia útil:
  - Calcula o fator diário:
  
    \[
    \text{fator diário} = (1 + \frac{\text{taxa anual}}{100})^{\frac{1}{252}}
    \]
- Multiplicação dos fatores diários (produtório) para obter o fator acumulado.
- **Truncamento**:
  - Fator diário: 8 casas decimais
  - Fator acumulado: 16 casas decimais
  - Valor atualizado: 8 casas decimais
- Taxa diária utilizada é sempre do último dia útil anterior.

---

## ✅ Testes de Unidade

- Criados utilizando **xUnit**.
- Banco de dados simulado usando `InMemoryDatabase`.
- Testes cobrem:
  - Cálculo correto do fator de juros.
  - Cálculo correto do valor atualizado.
  - Tratamento de dias úteis.

---

## 👨‍💻 Autor

Desenvolvido por [Thomas Ferraz](https://github.com/ThomasFerraz).

---

# 📌 Observações

- O projeto considera os princípios de clean code e separação de responsabilidades.
- Segue a especificação fornecida para cálculo de aplicações com indexador pós-fixado.
- A aplicação está preparada para extensão futura (ex: mais indexadores, cálculos avançados).

---

# 🚀 Instruções Finais

- Após clonar o projeto e restaurar pacotes, execute normalmente pelo Visual Studio ou via terminal.
- O projeto cria automaticamente o banco SQLite se ele não existir.
- Documentação interativa disponível via **Swagger**.

---
