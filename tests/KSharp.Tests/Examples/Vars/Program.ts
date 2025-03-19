import { System, Ks } from "@danfma/ksharp";

const a: System.Int32 = Ks.toInt32(1);
const b: System.Int32 = Ks.toInt32(2);
const c: System.Int32 = Ks.opAdd(a, b);
