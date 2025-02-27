fun main() {
  fizzBuzzTo(100)
} 

fun isDivisibleBy(number: Int32, divisor: Int32): Bool {
  if divisor == 0 {
    return false
  }
  
  return number % divisor == 0
}

fun fizzBuzz(number: Int32) {
  if isDivisibleBy(number, 15) {
    writeLine("FizzBuzz")
  } else if isDivisibleBy(number, 3) {
    writeLine("Fizz")
  } else if isDivisibleBy(number, 5) {
    writeLine("Buzz")
  } else {
    writeLine(number)
  }
}

fun fizzBuzzTo(number: Int32): Void { // Void is the equivalent of Unit in Kotlin, and it's optional
  foreach i in 1..number {
    fizzBuzz(i)
  }
}
