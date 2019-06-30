# DM106_Final
Repo destinado à hospedagem do trabalho final da disciplina DM106.

## Informações de acesso

Segue abaixo algumas informações para acesso. Algumas informações foram adicionadas aqui apenas por questões de praticidade.

_**Endpoint (URL):** http://dm106tf.azurewebsites.net_

**Banco de dados:** *DM106TF_db*  
**Usuário:** *fgadmin*  
**Senha:** *DM106@Final*

## Admin do sistema (não vinculado com CRM):

Administrador do sistema, possui autorização elevada na execução das operações.

**E-mail:** matilde@siecola.com  
**Senha:** Matilde#7  

## Usuários pré cadastrados (vinculados com CRM):

Usuários do sistema, Possuem autorização limitada as operações.  
Existem outros usuários cadastrados, mas não estão aqui citados pois não possuem vinculo com CRM.

**E-mail:** luiza@gmail.com  
**Senha:** Luiza#1  
**Cep:** 12239-018  

**E-mail:** jgilsonsi@gmail.com  
**Senha:** Jgilsonsi#1  
**Cep:** 37660-000  

**E-mail:** paulinhoed@gmail.com  
**Senha:** Paulinho#1  
**Cep:** 37540-000  

## Produtos pré cadastrados:

Alguns produtos já se encontram pré-cadastrados no banco. Estes já estão com valores plausíveis para calculo de frete por meio do serviço dos Correios. Caso adicione novos produtos, favor atentar-se aos valores dos atributos para que não gerem erros ao efetuar o calculo do frete.

**Id:** 1  
**Nome:** produto 1  

**Id:** 2  
**Nome:** produto 2  

**Id:** 3  
**Nome:** produto 3  

**Id:** 4  
**Nome:** produto 4  

Os produtos pré cadastrados foram todos testados pelo site oficial do serviço: http://ws.correios.com.br/calculador/CalcPrecoPrazo.asmx?op=CalcPrecoPrazo
Existe também a documentação desse serviço: https://www.correios.com.br/a-a-z/pdf/calculador-remoto-de-precos-e-prazos/manual-de-implementacao-do-calculo-remoto-de-precos-e-prazos

## Pedidos pré cadastrados:

Devido à limitação do serviço dos Correios, todos os produtos foram atualizados com medidas mais plausíveis de aceitação de calculo de frete.  
No entanto, ainda é possivel, mediante a quantidade de itens do pedido, que dê erro no calculo do frete por conta das dimensões (erro é retornado na requisição), e por isso segue abaixo alguns JSON's que funcionaram nos testes:

`{
    "OrderItems": [
        {
            "quantidade": 3,
            "ProductId": 1
        }
    ],
    "userName": "luiza@gmail.com"
}`

`{
    "OrderItems": [
        {
            "quantidade": 2,
            "ProductId": 1
        },
        {
            "quantidade": 1,
            "ProductId": 2
        },{
            "quantidade": 2,
            "ProductId": 3
        }
    ],
    "userName": "paulinhoed@gmail.com"
}`

`{
    "OrderItems": [
        {
            "quantidade": 1,
            "ProductId": 2
        },
        {
            "quantidade": 2,
            "ProductId": 4
        }
    ],
    "userName": "jgilsonsi@gmail.com"
}`

Note que há um para cada usuário de CRM vinculado.

## Teste de requisições via POSTMAN:

Na pasta `/Postman` existe uma collection do post que eu exportei. Nela já estão configuradas as requisições, bastando apenas alterar o body, url, dependendo da operação de deseja executar.

### Observação:

Conforme conversado por e-mail, para fechar o fechamento do pedido foi feito por meio de uma operação PUT.  
Para fechar o pedido de ID _15_, realizar um PUT na URL `/api/orders/15`.
