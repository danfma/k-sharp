fun fibonacci(n: UInt32): UInt32 {
  if n <= 1 {
    return n
  }
  
  return fibonacci(n - 1) + fibonacci(n - 2)
}
