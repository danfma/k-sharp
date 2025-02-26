import { render } from "solid-js/web"
import { failWith } from 'soil'
import { NullReferenceException } from 'soil/System'

const container = document.getElementById("root") 
    ?? failWith(new NullReferenceException("document.getElementById(\"root\")"))

render(() => <Counter start={0} />, container)

export function Counter(props) {
    const {
        start = 0,
    } = props
    
    const [count, setCount] = createSignal(start)
    const decrement = () => setCount(count() - 1)
    const increment = () => setCount(count() + 1)
    
    return (
        <div className="container">
            <button type="button" onClick={decrement}>
                Decrement
            </button>
            
            Count: {count()}
            
            <button type="button" onClick={increment}>
                Increment
            </button>
        </div>
    )
}
