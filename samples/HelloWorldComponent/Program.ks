
class HelloWorld : Component {
  let name: String required

  func render(): JsxElement {
    return Div(
      className = "container",
      $"Hello, $name!"
    )
  }
}

class Counter : Component {
  let start: Int32 = 0
  
  override func render(): JsxElement {
    let (count, setCount) = createSignal(.start)
    
    return Div(
      className = "container",
      Button(
        onClick = { setCount(count + 1) },
        $"Count: $count"
      )
    )
  }
}

let app = Div(
  HelloWorld(name = "Daniel"),
  Counter(count = 0),
  Counter()
)

// transformed like

const app = (
  <div>
    <HelloWorld name="Daniel" />
    <Counter count={0} />
    <Counter />
  </div>
)

function HelloWorld({ name }) {
  return <div className="container">Hello, {name}!</div>
}

function Counter(props) {
  const { 
    count = 0 
  } = props
  
  const [count, setCount] = createSignal(0)
  
  return (
    <div className="container">
      <button onClick={() => setCount(count + 1)}>Count: {count}</button>
    </div>
  )
}