import { System, Ks } from "@danfma/ksharp";
// This is a top-level statement example in TypeScript
import Console = System.Console;

// Exemplo de top-level statements
Console.writeLine("Hello, World!")

const a: System.Int32 = Ks.toInt32(10);
const b: System.Int32 = Ks.toInt32(20);

Console.writeLine(Ks.opAdd(a, b));

if (a > b) {
  Console.writeLine("a is greater than b")
} else {
  Console.writeLine("b is greater than or equal to a")
}