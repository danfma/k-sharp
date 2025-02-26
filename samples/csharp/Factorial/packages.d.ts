declare module "@k-sharp/core" {
    export namespace Runtime {
        export namespace Converters {
            export function parseInt(value: string, radix?: number): number;
        }
    }
}

declare module "@k-sharp/mscorlib/System" {
    export class Console {
        static writeLine(value: any, ...args: any[]): void;
    }
    
    export namespace Operator {
        export const plus: unique symbol;
        export const minus: unique symbol;
        export const times: unique symbol;
        export const lte: unique symbol;
    }
    
    export class Object {
        constructor();
        equals(other: Object): boolean;
        getHashCode(): number;
        toString(): number;
    }
    
    export type INumber<T> = T & {
        [Operator.plus](other: T): T;
        [Operator.minus](other: T): T;
        [Operator.times](other: T): T;
        [Operator.lte](other: T): boolean;
    }

    export class Int32 extends Object implements INumber<Int32> {
        constructor(value: number);

        static parse(value: string, radix?: number): Int32;
        static valueOf(value: any): Int32;
        
        static [Operator.plus](a: Int32, b: Int32): Int32;
        static [Operator.minus](a: Int32, b: Int32): Int32;
        static [Operator.times](a: Int32, b: Int32): Int32;
        static [Operator.lte](a: Int32, b: Int32): boolean;
    }
}
