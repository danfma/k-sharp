using System.Console.*

val n = args[0].toInt32()

writeLine(factorial(n))

fun factorial(n: Int32): Int32 {
  if n <= 1 {
    return 1
  }
  
  return n * factorial(n - 1)
}
