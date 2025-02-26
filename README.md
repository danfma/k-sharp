# K# (k-sharp)

Playground to create a new language named K# (Kotlin mixed and sharped with other languages) which should be compiled to
.NET, JavaScript, and maybe others.

The language is hardly inspired by the Kotlin with bits of Swift, TypeScript, Scala, and C#.

While the baseline is the .NET itself, I wanted it to bring power to the JS world too. First, because I want to use the
language full-power in the browser, and second, because I want to share the same codebase between the backend and the frontend,
and, in the future, we have one for it, we can try to follow a path like Kotlin did with its multiplatform support.

> This project is for FUN, LEARNING, and EXPERIMENTING with the knowledge area of
> Compilers in Computer Science (except that I will start skipping the creation of
> the parser by myself because I really want to work with the generated stuff. And,
> that's is the reason why I converted the project from F# to C# again. Maybe in the
> future I do that! Maybe... :D).

## Goals

- [x] Investigate a parser to play with (using Irony for now);
- [x] Investigate how to parse and roughly generate the AST;
- [x] Investigate how to create partial tests to verify the generated AST;
- [ ] Create a semantic analyzer to verify the generated AST;
- [ ] Translate code to JavaScript;
- [ ] Translate code to C#;
- [ ] Translate code to .NET IL;
- [ ] Translate code to WebAssembly (maybe);
- [ ] Check other possible backend targets;

## The K# language

K# is almost the Kotlin language with a few changes in the syntax and, obviously, a smaller set of features. I'm
creating it for fun, I'm a father of a newborn baby (yes, I am not sleeping well by these days!), thus I don't have
enough time to create something better for now...

### Value declaration (non-reassignable variables)

- `val x = 10`
- `val x: Int = 10`
- `val name = "Daniel"`
- `val finished = false`
- `val finished: Bool = default`

### Variable declaration (mutable)

- `var x = 10`
- `var x: Int = 10`
- `var name = "Daniel"`
- `var finished = false`
- `var finished: Bool = default`

### Primitive Types table

| K# Type       | .NET Type           | TypeScript Type |
|---------------|---------------------|-----------------|
| `Char`        | `CharUtf16`         | `CharUtf8`      |
| `Utf8Char`    | `Byte`              | `Character`     |
| `Utf16Char`   | `Char`              | `Character`     |
| `Int8`        | `SByte`             | `Number`        |
| `Int16`       | `Int16` (`short`)   | `Number`        |
| `Int32`       | `Int32` (`int`)     | `Number`        |
| `Int64`       | `Int64` (`long`)    | `BigInt`        |
| `UInt8`       | `Byte`              | `Number`        |
| `UInt16`      | `UInt16` (`ushort`) | `Number`        |
| `UInt32`      | `UInt32` (`uint`)   | `Number`        |
| `UInt64`      | `UInt64` (`ulong`)  | `BigInt`        |
| `Float32`     | `Single` (`float`)  | `Number`        |
| `Float64`     | `Double` (`double`) | `Number`        |
| `Bool`        | `Bool`              | `Boolean`       |
| `String`      | `String`            | `String`        |
| `Utf8String`  | `Byte[]`            | `String`        |
| `Utf16String` | `String`            | `String`        |

### Special types table

| K# Type  | .NET Type | TypeScript Type    |
|----------|-----------|--------------------|
| `Any`    | `Object`  | `any`              |
| `Unit`   | `Void`    | `void`             |
| `Number` | `None`    | `Number`           |
| `Range`  | `Range`   | `[number, number]` |
| `Index`  | `Index`   | `Index`            |

### Tuples

**Named tuples**

```
type PersonAge = (name: String, age: UInt8)
```

**Unnamed tuples**

```
type PersonAge = (String, Uint8)
```

### Nullability

By default, the types above don't accept `null`. You have to explicitly define them
as `nullable` to make that work.

For example:

```
val checked: Bool? = null
val enabled: Bool = null // it will show an error
```

> Note: In C#, we have nullable reference types and nullable value types.
> In K#, we only have nullable types which are mapped to the first or second types before,
> when targeting .NET.

> Note: In TypeScript, we nullable types can be supported as a union type with `null` or `undefined`.

### Expressions

* Example: sum of two numbers:

```
val x = 10
val y = 20
val sum = x + y
```

* Example: sum of two numbers using a single variable:

```
var sum = 0
sum = sum + 10
sum += sum + 20
```

#### Bit operators

* Binary operators

- `<< N` shift left N bits
- `>> N` shift right N bits
- `<<< N` rotate left N bits ? (IDEA)
- `>>> N` rotate right N bits ? (IDEA)

#### Number Operators

* Unary operators

- `-` negates a number
- `not` negates a boolean value

* Binary operators

- `+` addition of two numbers
- `-` subtraction of two numbers
- `*` multiplication of two numbers
- `/` division of two numbers
- `%` remainder of the division between two numbers

#### Boolean Operators

- `!=` not equal
- `==` equal
- `<` less than
- `<=` less or equal than
- `>` greater than
- `>=` greater or equal than
- `refEquals` reference equality

#### Logic Operators (used as text for readability)

- `not` logical NOT
- `and` logical AND
- `or` logical OR
- `xor` logical XOR

### Control flow

#### Conditions

* if only

```
if value == true {
  // do something
}
```

* if / else

```
if value == true {
  // do something
} else {
  // or do this
}
```

* if / else chain

```
if not goodBook {
  // discard it
} else if hasMore {
  // take another one!
}
```

* if / else if / else

```
if not goodBook {
  // discard it
} else if gift {
  // put it on the cabinet
} else {
  // throw it away
}
```

#### when statement

```
when grade {
  'A' -> print("Superb!")
  'B' -> print("Very good!")
  'C' or 'D' -> print("We need to study more...")
  else -> print("Daddy is coming! Run!!")
}
```

#### while statement

```
while not stack.empty {
  print(">> ${stack.pop()}")
}
```

#### for statements

```
for var i = 0; i < 10; i++ {
  // old for style for
}

// by default, item type will be infered from the list item; also, it's read-only
foreach item in list {
  // do something
}

// each value from the list will be casted to Int
foreach item as Int in list {
  // do something
}

// in this case, the item is a mutable copy of the list item
foreach var item in list {
  // do something
}

var i = 0

do {
    // do something
} while i++ < 20
```

Enumerables:

```
foreach i in range(from = 0, to = 1, by = 1) {
}
```

#### if expressions

```
// ternary-like
val grade = if points >= 9 then 'A' else 'B'

// chained ifs
val grade = if points >= 9 then 'A' else if points >= 8 then 'B' else 'C'

// multi-line chained ifs
val grade =
  if points >= 9 then 'A'
  else if points >= 8 then 'B'
  else if points >= 7 then 'C'
  else 'D' 
```

#### when expression

```
val numberAsString = when digit {
  0 -> "zero"
  1 -> "one"
  2 -> "two"
  3 -> "three"
  else -> "four"
}
```

## Namespaces

```
// per-file namespace
namespace My.Helpers

// use non-ambiguous namespace inferred from the referenced libraries
using Kool
// explicitly specify the assembly or package and then import its namespace
using from "mscorlib" System.Numerics
```

## Classes

All classes are reference types by default. However, there are value classes that are stored in the stack.
Additionally, there are other variants like data classes, which are types that will provide a default implementation
for `equals`, `hashCode`, and `toString` methods, and some additional helpers methods for destructuring.

The syntax is:

```
[modifier = public] [partial] [value] [data] class $identifier [: $baseTypeOrInterface] {
  ...class_members*
}
```

```
class Person {
  // mutable fields (use val for readonly)
  var name: String = ""
  // or
  var name = "" // the type is inferred from the assignment
  var surname: String? = null
  var age: Int = 0
  // or
  var age = 0 // the type is inferred from the assignment
  
  val fullName: String
    get() {
        return .surname is not null ? $"{.name} {.surname}" : .name
    }

  // or
  val fullName get() = if .surname is not null then $"{.name} {.surname}" else .name

  fun setName(name: String, surname: String, age: Int? = null) {
    .name = name
    .surname = surname

    if age is not null {
      .age = age // automatic casting from Int? to Int
    }
  }

  override fun toString(): String {
    return (
      $"""
      Person {
        {nameof(.name)} = "{.name}"
      }
      """
    )
  }

  // or
  override fun toString(): String =
    $"""
    Person {
      {nameof(.name)} = "{.name}"
    }
    """
}
```

### Fields

Fields are defined using the `var` or `val` keywords. The `var` keyword is used for mutable (non readonly) fields,
while the `val` keyword is used for readonly fields. The type can be inferred from the assignment.

The syntax is: `[modifier=public] (let|var) identifier [: Type] [= expression]`

Note:

* The expression assignment can only be omitted for fields of a value class type, or a nullable reference type, in this
  case the value assigned will be `default(Type)`.

Example:

```
class Point {
  var x: Float32 = 0
  var y = 0f                        // public field (default), type inferred from the assignment
  private val scale = 1f            // private field
}

class Math {
  val pi = 3.14159265359
}
```

### Properties

Properties are defined using an extension of the field declaration which includes
a getter and/or a setter block.

The syntax is: `(let|var) [modifier=public] [initonly] [required] identifier [: $type] ([= $value] | lateinit) $getter set $setter`

Example:

```
class Example {
  var x = 0f
  var y = 0f
  private var _steering = 0f

  // full definition
  let length: Float32
    get() {
      return Math.sqrt(.x * .x + .y * .y)
    }

  // simplified definition of code above, type omitted
  let length get() -> Math.sqrt(.x * .x + .y * .y)

  // property with backing field, type omitted and inferred from the assignment
  var steering = 0f
    get() {
      return ._steering
    }
    set {
      ._steering = Math.clamp(value, -maxSteering, maxSteering)
    }

  companion {
    const maxSteering = 20.0
  }
}
```

#### auto property

An auto property is a property that has a default implementation for the getter and setter methods.

```
class Person {
  var name = ""
    get
    set
}

val p = Person()
p.name = "Daniel"
writeLine(p.name) // prints "Daniel"
```

#### field keyword

The `field` keyword can be used to access the backing field of a property, which will be automatically generated for auto or semi auto properties.

```
class Person {
  var name = ""
    get() -> field
    set(value) -> field = value

  // which is equivalent to
  var private _name@ = "" // backing field generated by the compiler not accessible from outside

  var name: String
    get() -> _name@
    set(value) -> _name@ = value
}
```

### init-only and required accessors

The `initonly` and `required` keywords can be used in conjunction with the `var` keyword to define properties that
can only be set during the object initialization.

```

// using field accessors
class Example {
  var x = 0f
    get() -> field
    init(value) -> field = value

  var y: Float32 = 0f
    get() -> field
    init(value) -> field = value

  var z: Float32
    get() -> field
    required init(value) -> field = value

  var w: Float32
    get() -> field
    required set(value) -> field = value

  /**
   * using auto-properties
   *
   * var x = 0f get, init
   * var y: Float32 = 0f get, init
   * var z: Float32 get, required init
   * var w: Float32 get, required set
   */
}

let example = Example1(x = 10, y = 20, z = 30, w = 40)

example.x = 10 // error because x is initonly
example.y = 10 // error because y is initonly
example.z = 10 // Ok because the z is mutable
example.w = 10 // error because w is initonly required

let example2 = Example1() // error because z and w are required
```

### Methods

```kotlin
class Calc {
  // static methods can be defined in a static block
  static {
    fun sum(a: Int, b: Int): Int -> a + b

    fun subtract(a: Int, b: Int) -> a - b

    fun multiply(a: Int, b: Int) -> a * b

    // the throws part is optional and merely a documentation that the method can throw an exception
    fun divide(a: Int, b: Int) throws DivideByZeroException -> a / b
    // or
    fun divide(a: Int, b: Int) throws -> a / b
    // or
    fun divide(a: Int, b: Int) -> a / b
  }
}

abstract class Greeter {
  abstract fun greet(name: String): String
}

class EnglishGreeter : Greeter {
  override fun greet(name: String): String -> "Hello, $name!"
}

class PortugueseGreeter : Greeter {
  override fun greet(name: String): String -> "Olá, $name!"
}
```

```typescript
import { Int, String, Runtime, assert } from '@k-sharp/runtime';

export class Calc {
  static sum(a: Int, b: Int): Int {
    return Runtime.add(a, b);
  }

  static subtract(a: Int, b: Int): Int {
    return Runtime.subtract(a, b);
  }

  static multiply(a: Int, b: Int): Int {
    return Runtime.multiply(a, b);
  }

  static divide(a: Int, b: Int): Int {
    return Runtime.divide(a, b);
  }
}

export abstract class Greeter {
  abstract greet(name: string): string;
}

export class EnglishGreeter extends Greeter {
  greet(name: string): string {
    return `Hello, ${name}!`;
  }
}

export class PortugueseGreeter extends Greeter {
  greet(name: string): string {
    return `Olá, ${name}!`;
  }
}
```

## Value Classes (aka structs)

All value classes are structs by default (in other words, they are stored into stack instead of heap).

```
value class Counter(var private value: Int = 0) {
  fun increment() {
    value++
  }

  operator implicit fun() = value
}

fun extend(counter: Counter) -> Counter {
  counter.increment()
  return counter
}

let source = Counter(10)
let result = extend(source)

print(source.value) // prints 0
print(result.value) // prints 1
print(if source === result then "equal" else "not equal") // prints "not equal"
```

> NOTE: TypeScript doesn't support the native definition of value classes, then we emulate it by creating new instances on every
mutation.

TypeScript:

```typescript
import { Int, String, Runtime, Operator, assert } from '@k-sharp/runtime';

export class Counter {
  constructor(private value: Int = 0) { }
  
  increment(): void {
    this.value++;
  }
  
  static [Operator.Implicit](self: Counter): Int {
    return self.value;
  }
}

export function extend(counter: Counter): Counter {
  counter = Runtime.copy(Counter, counter);
  counter.increment();
  return counter;
}

const source = new Counter(10);
const result = extend(source);

writeLine(source.value); // prints 0
writeLine(result.value); // prints 1
writeLine(source === result ? "equal" : "not equal"); // prints "not equal"
```

## Enum

```
// simple enum
enum ProcessStatus {
  New,
  Ready,
  Run,
  Terminated
}

var x = ProcessStatus.New
x = ProcessStatus.Ready
```

## Generics

```
fun sum<T>(a: T, b: T) -> Int where T extends INumber<T> {
  return a + b
}
```

```typescript
import { type ReifiedCall, type TypeInfo, Runtime } from '@soil/runtime';

export function sum$T<T>($call: ReifiedCall<{ T: TypeInfo<T> }>): (a: T, b: T) => Int {
  return Runtime.createGenericFunction($call, (a: T, b: T) => a + b);
}
```

## Samples

### Counter class

**K#**

```
namespace Counting

class Counter(let count: Int) {
    let double with get() = count * 2

    fun increment() = Counter(count + 1)
    fun decrement() = Counter(count - 1)

    operator fun implicit() = count
}

let counter = Counter(10)
let value: Int = counter
```

**C#**

```csharp
namespace Counting;

Counter counter = new(10);
int value = counter;

public class Counter(int count)
{
    public int Count { get; } = count;
    public int Double => Count * 2;

    public Counter Increment() => new(Count + 1);
    public Counter Decrement() => new(Count - 1);

    public static operator implicit int(Counter counter) => counter.Count;
}
```

**TypeScript**

```typescript

// imported
export type Int = number;

// probably it's better to not use a namespace here but another structure
export namespace Sample {
    export const Ctr$Int = Symbol.for('new Counter(Int)');
    export const OpImplicit$Counter$Int = Symbol.for('fun implicit (Int) -> Counter');

    export class Counter {
        constructor(public readonly count: Int) { }

        get double(): Int {
            return this.count * 2;
        }

        increment(): Counter {
            return new Counter(this.count + 1);
        }

        decrement(): Counter {
            return new Counter(this.count - 1);
        }

        static [OpImplicit$Counter$Int](counter: Counter): Int {
            return counter.count;
        }

        static [Ctr$Int](count: Int): Counter {
            return new Counter(count);
        }
    }
}

// translated code to keep semantic usage
const counter = Sample.Counter[Sample.Ctr$Int](10);
const value: Int = Sample.Counter[Sample.OpImplicit$Counter$Int](counter);

```

### Fibonacci function

**K#**

*Fibonacci.soul*

```
namespace Sample

fun fibonacci(n: UInt) = if n <= 2 then n else fibonacci(n - 1) + fibonacci(n - 2)
```

**C#**

```csharp
namespace Sample;

public static class FibonacciModule
{
    public static uint Fibonacci(uint n) =>
        n <= 2 ? n : Fibonacci(n - 1) + Fibonacci(n - 2);
}
```

**TypeScript**

```typescript
export type UInt = number;

export namespace Sample {
    export function fibonacci(n: UInt): UInt {
        return n <= 2 ? n : fibonacci(n - 1) + fibonacci(n - 2);
    }
}
```

### JSX

**K#**

```
using Solid
using Solid.Html // provided by the Solid library binding

@file:Js(ignoreNamespace = true)

namespace Sample

class Counter : JsxNode {
  let initialCount: Int? = null with get, init

  fun render() -> JsxNode {
    let (value, setValue) = createSignal(initialCount ?? 0)

    let increment = { e' ->
      setValue(value() + 1)
    }

    // translated to
    return Div(
      className = "counter",
      children = [
        Button(
          type = "button",
          onClick = increment,
          children = [
            Text(value())
          ]
        )
      ]
    )
  }
}

class App : JsxNode {
  fun render() -> JsxNode {
    return Div(
      className = "app",
      children = [
        Counter(initialCount = 10)
      ]
    )
  }
}

run<App>()
```

```typescript jsx

// require soil-solid-transformer

import { createSignal, render } from 'solid-js';

export type CounterProps = {
  initialCount?: number | null;
}

export function Counter(props: CounterProps): JSX.Element {
  const [count, setCounter] = createSignal(props.initialCount ?? 0);
  const increment = () => setCounter(value() + 1);

  return (
    <div class="counter">
      <button type="button" onClick={increment}>
        {count()}
      </button>
    </div>
  );
}

export function App(): JSX.Element {
  return (
    <div class="app">
      <Counter initialCount={10} />
    </div>
  );
}

// the first call will return a cached function configured with the provided type parameters
run({ T: Runtime.getType(App) })();

```

```csharp

using Solid;
using Solid.Html;

Run<App>();

public class Counter : JsxNode
{
    public int? InitialCount { get; init; } = null;

    public JsxNode Render()
    {
        var (count, setCounter) = createSignal(InitialCount ?? 0);
        var increment = () => setCounter(count() + 1);

        return new Div {
            ClassName = "counter",
            Children = [
                new Button {
                    Type = "button",
                    OnClick = increment,
                    Children = [
                        new Text(count.Value))
                    ]
                }
            ]
        };
    }
}

public class App : JsxNode
{
    public JsxNode Render()
    {
        return new Div {
            ClassName = "app",
            Children = [
                new Counter {
                    InitialCount = 10
                }
            ]
        };
    }
}
```

### Point class with operator overloading

```
namespace Sample

value data class Point(val x: Float32, val y: Float32) {
    val length: Float32 get() => MathF.sqrt(x * x + y * y)

    func add(other: Point): Point => Point(x + other.x, y + other.y)
    func subtract(other: Point): Point => Point(x - other.x, y - other.y)

    static func operator +(left: Point, right: Point): Point => left.add(right)
    static func operator -(left: Point, right: Point): Point => left.subtract(right)
    static func operator ==(left: Point, right: Point): Bool => left.Equals(right)
    static func operator !=(left: Point, right: Point): Bool => !left.Equals(right)
}
```

* Pass by value vs ref

```
func passByValue(value: Int) {
  value += 1
}

func passByRef(ref value: Int) {
  value += 1
}

var x = 10

passByValue(x)
print(x) // prints 10
passByRef(ref x)
print(x) // prints 11
```

* Closure

```
// shorter-version
func inc(increment: Int = 1) => (value: Int) => value + increment

// inner function inferred from outer type
func inc(increment: Int = 1): Func<(Int), Int> {
  return value => value + increment
}

// all typed
func inc(increment: Int = 1): Func<(value: Int), Int> {
  return (value: Int): Int => value + increment
}
```

```
func sum(array: Int[]): Int {
  var sum = 0
  
  for value in array {
    sum += value
  }
  
  return sum
}

func sum(array: Int[]): Int {
  return array.sum()
}
```

```
record class Point(
  val x: Int = 0
  val y: Int = 0
) {
  constructor(other: Point) 
    : this(other.x, other.y)
  
  prop length: Int => Math.sqrt(x * x + y * y)
}

// same as
class Point(x: Int = 0, y: Int = 0) {
  private val _x: Int
  private val _y: Int
  
  init {
    _x = x
    _y = y
  }
  
  constructor(other: Point)
    : this(other.x, other.y)
    
  // full getter definition
  prop x: Int {
    get {
      return _x
    }
  }
  
  // simplified getter definition
  prop y: Int {
    get => _y
  }
  
  // fully-simplified getter-only definition
  prop length: Int => Math.sqrt(x * x + y * y)
  
  override func equals(other: Any): Bool {
    return other is Point and x == other.x and y == other.y
  }
  
  override func getHashCode(): Int =>
    HashCode.Combine(x, y)
    
  open func deconstruct(): (Int, Int, Int) => (x, y, length)
}
```

// Possible React Integration through macro and protocols ???

```
protocol interface CounterProps for JsObject {
  val initialCount: Int? = null
}

val Counter = component!{ (props: JsObject with CounterProps): JsxElement => {
  val [count, setCount] = useState(props.initialCount ?? 0)
  
  val decrement = useCallback(
    { setCount({ it - 1 }) },
    []
  )
  
  val increment = useCallback(
    { setCount({ it + 1 }) },
    []
  )

  return jsx!{
    <div>
      <button type="button" onClick={decrement}> - </button>
      <span>{count}</span>
      </button> type="button" onClick={increment}> + </button>
    </div>
  }
}

// maybe using types and macros
[macro:FuncComponent]
class Counter {
  static operator func invoke(props: JsObject with CounterProps): JsxElement {
    val [count, setCount] = useState(props.initialCount ?? 0)
  
    val decrement = useCallback(
      { setCount({ it - 1 }) },
      []
    )

    val increment = useCallback(
      { setCount({ it + 1 }) },
      []
    )

    return jsx!{
      <div>
        <button type="button" onClick={decrement}> - </button>
        <span>{count}</span>
        </button> type="button" onClick={increment}> + </button>
      </div>
    }
  }
}


// maybe using a bultin DSL
[FuncComponent]
object Counter implements IFuncComponent {
  static operator func invoke(props: JsObject with CounterProps): JsxElement {
    val [count, setCount] = useState(props.initialCount ?? 0)
  
    val decrement = useCallback(
      { setCount({ it - 1 }) },
      []
    )

    val increment = useCallback(
      { setCount({ it + 1 }) },
      []
    )

    return jsx {
      div {
        button(type="button", onClick=decrement) {
          text " - "
        }
        span {
          text count
        }
        button(type="button", onClick=increment) {
          text " + "
        }
      }
    }
  }
}

// transpiled to javascript ES2015
export class CounterPropsProtocol {
  static get_initialCount(source, defaultValue = null) {
    return source.initialCount ?? defaultValue
  }
}

const Counter = (props) => {
  const [count, setCount] = useState(CounterPropsProtocol.get_initialCount(props) ?? 0);
  
  const decrement = useCallback(
    () => { setCount(it => it - 1) },
    []
  );
  
  const increment = useCallback(
    () => { setCount(it => it + 1) },
    []
  );

  return (
    <div>
      <button type="button" onClick={decrement}> - </button>
      <span>{count}</span>
      </button> type="button" onClick={increment}> + </button>
    </div>
  );
};
```

> TODO Describe the language here

