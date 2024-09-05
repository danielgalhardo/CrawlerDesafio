# Desafio

O desafio consiste no desenvolvimento de um RPA simples que realiza uma busca automatizada no site da Alura (https://www.alura.com.br/) e grava os resultados em um banco de dados (mais informações abaixo).

## Método Utilizado

- **Framework:** .NET 7, em C#
- **Ferramentas:**
  - Selenium WebDriver
  - Serilog para disponibilizar logs
  - Injeção de dependências para utilização dos serviços utilizados na aplicação
  - Entity Framework Core
  - SQLite

## Padrão Utilizado

- **DDD (Domain-Driven Design)**

## Decisões Tomadas

Por já trabalhar há bastante tempo com o Selenium, optei por usá-lo como ferramenta para automatizar a busca. Através dele, utilizei diferentes métodos para localizar elementos na tela, sendo eles:

- Xpath
- Css Selector

## Fluxo

1. **Configuração:** Através do `appsettings`, é definido um valor para ser buscado. Caso a string seja nula, o valor buscado será "RPA".
2. **Inicialização:** Utilizei o DriverManager para buscar remotamente a versão correta do driver para inicializar o navegador com suas configurações.
3. **Busca:** O site é acessado, a palavra-chave é buscada e o scraping das informações começa.
4. **Scraping:** Foi utilizada uma classe para representar o Curso e suas informações, incluindo:
   - Nome do curso
   - Link do curso
   - Descrição do curso
   - Carga horária
   - Instrutor

   A busca é feita por todas as páginas, salvando inicialmente o nome, a descrição e o link. Após o scraping de todas as páginas, cada curso tem seu link acessado para scraping das informações sobre os instrutores e a carga horária. É feito um filtro para buscar apenas títulos que contenham "Curso" ou "Formação", excluindo podcasts e artigos que não possuem as informações solicitadas.

## Persistência dos Dados

Para persistência dos dados, foi utilizado o SQLite para a criação do banco de dados, facilitando o armazenamento das informações em uma tabela. O Entity Framework Core foi utilizado para manipulação dos dados, pois o uso de migrations facilita a manutenção a longo prazo da aplicação.

## Conclusão

Com isso, foi possível buscar os dados de cursos e formações do Alura, capturando suas principais informações e persistindo os dados utilizando o SQLite.
