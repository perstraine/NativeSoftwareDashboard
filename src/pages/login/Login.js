import styles from "./Login.module.css"
import Logo from './assets/Logo.png'
import { useState } from 'react'

export default function Login() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    function handleChange(e) {
        if (e.target.name === 'email') {
            setEmail(e.target.value)
        } else {
            setPassword(e.target.value)
        }
    }
    function buttonClick() {
        console.log(`email: ${email}, pass: ${password}`)
    }
    return (
      <>
        <div id={styles.outerContainer}>
          <div id={styles.innerContainer}>
            <div id={styles.logo}>
              <img src={Logo} alt="Logo" />
            </div>
            <div id={styles.title}>Dashboard Login</div>
            <div id={styles.fields}>
              <form>
                <div className={styles.field}>
                  <input
                    type="email"
                    placeholder="Email"
                    value={email}
                    name="email"
                    onChange={handleChange}
                  />
                </div>
                <div className={styles.field}>
                  <input
                    type="Password"
                    placeholder="Password"
                    value={password}
                    name="password"
                    onChange={handleChange}
                  ></input>
                </div>
              </form>
            </div>
            <div id={styles.button} onClick={buttonClick}>Login</div>
          </div>
        </div>
      </>
    );
}

