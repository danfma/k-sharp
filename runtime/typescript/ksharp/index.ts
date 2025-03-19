import * as System from "./assemblies/mscorlib";

export function Runtime_toInt32 (value: number): System.Int32 {
    return <System.Int32>(value | 0);
}

export function Operator_add(a: System.Int32, b: System.Int32): System.Int32 {
    return <System.Int32>(a + b);
}

export function Operator_toString(value: System.Int32): System.String {
    return String(value) as System.String;
}

export * as System from "./assemblies/mscorlib";
