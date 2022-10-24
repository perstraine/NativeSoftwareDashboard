import axios from 'axios';
import styles from "./Login.module.css"
import Logo from './assets/Logo.png'
import { useEffect, useState } from 'react'


export default function Login() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')

    useEffect(() =>{
      axios.get("https://localhost:7001/api/Users")
      .then((response) => {
          console.log(response.data);
      })
    }, [])

    function handleChange(e) {
        if (e.target.name === 'email') {
            setEmail(e.target.value)
        } else {
            setPassword(e.target.value)
        }
    }

    const buttonClick = (e) => {
      e.preventDefault();
      setEmail(email);
      setPassword(password);
      console.log(`email: ${email}, pass: ${password}`)
      axios.post('https://localhost:7001/Auth/login', {
        email:email,
        password:password
      })
        .then((response) => {
          //should return token
          console.log(response);
        })
        .catch((error) => {
          console.log(error);
          console.log('some errorrrrr....');
        });
    };

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

