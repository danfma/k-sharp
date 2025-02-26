import {Console, Int32, Operator} from "@k-sharp/mscorlib/System";

const { writeLine } = Console;
const n = Int32.parse(process.argv[2], 10);

writeLine(factorial(n));

export function factorial(n: Int32): Int32 {
    if (Int32[Operator.lte](n, Int32.valueOf(1))) {
        return Int32.valueOf(1);
    }

    return Int32[Operator.times](n, factorial(Int32[Operator.minus](n, Int32.valueOf(1))));
}