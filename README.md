
## Exemplo  de  Cache  em  Memória  com  C#  e  ASP.NET  Core

Este  repositório  demonstra  a  implementação  de  cache  em  memória  em  uma  aplicação  ASP.NET  Core  utilizando  a  interface  IMemoryCache.  O  código  e  os  conceitos  apresentados  são  baseados  nas  excelentes  aulas  DotLive  por  Luís  Dev,  disponíveis  em  [https://metododotnet.luisdev.com.br](https://metododotnet.luisdev.com.br).

### Tecnologias  Empregadas

-   C#
    
-   ASP.NET  Core
    
-   Entity  Framework  Core
    
-   Microsoft.Extensions.Caching.Memory
    

### Estrutura  do  Projeto

-   **DotLive.Cacching.Contexto:**  Define  o  contexto  do  Entity  Framework  Core  para  acesso  ao  banco  de  dados.
    
-   **DotLive.Cacching.Models:**  Contém  o  modelo  Produto  que  representa  um  produto  no  sistema.
    
-   **DotLive.Cacching.Controllers:**  Implementa  o  controlador  ProdutosController  com  ações  para  gerenciar  produtos  e  demonstrar  o  uso  do  cache  em  memória.
    

### Implementação  do  Cache

O  ProdutosController  utiliza  o  serviço  IMemoryCache  para  armazenar  em  cache  a  lista  de  produtos  e  produtos  individuais.  As  chaves  de  cache  são  definidas  como  constantes:

-   KEY_CHACHE_PROODUTOS:  Armazena  a  lista  completa  de  produtos.
    
-   KEY_CHACHE_PTODUTO_ID:  Armazena  um  produto  específico  pelo  ID  (a  chave  é  construída  dinamicamente  com  o  ID).
    

As  ações  do  controlador  demonstram  como:

-   **Verificar  se  os  dados  estão  em  cache:**
    
    ```
    if (!_cache.TryGetValue(KEY_CHACHE_PROODUTOS, out produtos))
    {
        // ... consultar o banco de dados e adicionar ao cache
    }
    ```
    

    
-   **Adicionar  dados  ao  cache:**
    
    ```
    _cache.Set(KEY_CHACHE_PROODUTOS, produtos);
    ```
    

    
-   **Atualizar  o  cache:**
    
    ```
    _cache.Set(KEY_CHACHE_PTODUTO_ID + id, produto);
    ```
    

    
-   **Recuperar  dados  do  cache:**
    
    ```
    var produto = await _cache.GetOrCreateAsync(..., async () => { ... });
    ```
    

    

> **Observação:**  A  expiração  do  cache  não  é  implementada  neste  exemplo,  mas  é  um  aspecto  importante  a  ser  considerado  em  cenários  reais.

### Ações  do  Controlador

-   **PostAsync:**  Adiciona  um  novo  produto  e  o  retorna.
    
-   **GetAllAsync:**  Retorna  a  lista  de  todos  os  produtos  (com  cache).
    
-   **GetByIdAsync:**  Retorna  um  produto  específico  pelo  ID  (com  cache).
    
-   **PutAsync:**  Atualiza  um  produto  existente  e  atualiza  o  cache.
    
-   **DeleteAsync:**  Remove  um  produto  existente.
    

### Observações

-   O  código  inclui  seções  comentadas  que  mostram  a  implementação  sem  cache  para  comparação.
    
-   O  cache  em  memória  é  adequado  para  cenários  onde  os  dados  são  compartilhados  por  uma  única  instância  da  aplicação.  Para  cenários  distribuídos,  considere  soluções  de  cache  distribuído.
    

### Como  Executar  o  Projeto

1.  Clone  o  repositório.
    
2.  Configure  a  conexão  com  o  banco  de  dados  no  arquivo  de  configuração.
    
3.  Execute  a  aplicação.
    
4.  Utilize  ferramentas  como  Postman  ou  curl  para  interagir  com  as  APIs  do  controlador.
    

### Estudos  e  Anotações

-   Compare  as  implementações  com  e  sem  cache  para  entender  o  impacto  do  cache  no  desempenho.
    
-   Analise  como  as  chaves  de  cache  são  construídas  e  utilizadas.
    
-   Observe  como  o  cache  é  atualizado  para  manter  a  consistência  dos  dados.
    
-   Considere  como  implementar  a  expiração  do  cache  e  estratégias  de  invalidação.
    
-   Pesquise  sobre  outras  opções  de  cache,  como  cache  distribuído,  para  cenários  mais  complexos.
    

Este  exemplo  serve  como  um  ponto  de  partida  para  explorar  o  uso  de  cache  em  memória  em  aplicações  ASP.NET  Core,  com  base  nos  ensinamentos  de  Luís  Dev.  Adapte  e  expanda  o  código  para  atender  às  necessidades  específicas  do  seu  projeto.
