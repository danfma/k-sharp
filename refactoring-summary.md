# Resultado da Refatoração do KSharp

## Distinção entre Funções e Métodos

Na refatoração do compilador KSharp, foi introduzida uma clara distinção entre os seguintes conceitos:

1. **IrFunction** - Representa uma função de nível superior (top-level function)
   - Usada para funções declaradas diretamente no escopo de um arquivo
   - Pertence a um módulo, não a uma classe
   - Implementa a ideia das funções de nível superior do Kotlin

2. **IrMethod** - Representa um método de uma classe/interface  
   - Pertence a uma declaração de classe ou interface
   - Pode ter modificadores como abstract, virtual, override
   - Semanticamente diferente das funções de nível superior

## Outras Melhorias de Nomenclatura

- Nome das classes seguem o padrão do Roslyn
- Prefixo 'Ir' para classes da representação intermediária
- Sufixo 'Syntax' para classes da AST
- Documentação adicionada a todas as classes


