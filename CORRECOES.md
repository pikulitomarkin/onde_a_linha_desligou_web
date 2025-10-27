# Correções Realizadas - Linhas Assis C1 e C2

## Problema Identificado

As linhas Assis C1 e C2 estavam usando a mesma lógica de busca de waypoints no GPX, causando conflitos pois ambas buscavam apenas pelo número da torre (ex: "1") ao invés dos códigos completos com prefixos (ex: "7350TO001" e "7330TO001").

## Correções Implementadas

### 1. Correção do Menu de Londrina
- **Arquivo:** `kmweb.py` (linha ~268)
- **Problema:** Linha duplicada com nome incorreto
- **Correção:** Removida a entrada duplicada "Linha Londrina - Sarandi" que usava a chave `lna_assis`

### 2. Adição das Linhas Assis na Lista de Prefixos
- **Arquivo:** `kmweb.py` (linha ~135)
- **Correção:** Adicionadas as chaves `"lna_assis"` e `"assis_c2_londrina_norte"` na lista de linhas que devem incluir prefixos

### 3. Modificação da Função `extrair_numero_torre`
- **Arquivo:** `kmweb.py` (linha ~238)
- **Correção:** Adicionada lógica para retornar o código completo quando `incluir_prefixo=True`
- **Resultado:** 
  - Assis C1: `7350TO001` → retorna `"7350TO001"`
  - Assis C2: `7330TO001` → retorna `"7330TO001"`

### 4. Correção da Função `buscar_torre_no_gpx`
- **Arquivo:** `kmweb.py` (linha ~184)
- **Correção:** Passa o parâmetro `incluir_prefixo` corretamente para a função `extrair_numero_torre`

## Resultado Final

Agora as linhas Assis C1 e C2 buscam waypoints distintos nos seus respectivos arquivos GPX:

- **Assis C1 (`lna_assis`)**: Busca waypoints como `"7350TO001"`, `"7350TO002"`, etc. no arquivo `lna_assis.gpx`
- **Assis C2 (`assis_c2_londrina_norte`)**: Busca waypoints como `"7330TO001"`, `"7330TO002"`, etc. no arquivo `assis_c2_londrina_norte.gpx`

## Arquivos GPX Necessários

Para que o sistema funcione corretamente, os arquivos GPX devem conter waypoints com os nomes completos:

- `static/resources/lna_assis.gpx`: waypoints nomeados como `7350TO001`, `7350TO002`, etc.
- `static/resources/assis_c2_londrina_norte.gpx`: waypoints nomeados como `7330TO001`, `7330TO002`, etc.

Data da correção: 6 de outubro de 2025
