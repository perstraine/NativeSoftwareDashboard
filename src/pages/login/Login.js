import axios from 'axios';
import styles from "./Login.module.css"
import Logo from './assets/Logo.png'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';

export default function Login() {
    const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [logintoken, setLoginToken] = useState('');
    const navigate = useNavigate();

  useEffect(() => {
    setLoginToken(localStorage.getItem("token"));
  if(logintoken)
    {const config = { headers: { Authorization: `Bearer ${logintoken}` } };
    axios.get("https://localhost:7001/api/Users",config)
      .then((response) => {
        console.log(response);
        if (response.data === 'User Authorised') {
          navigate('/dashboard');
          console.log('User Authorised');
        }
      })}
  }, [logintoken])

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
      axios.post('https://localhost:7001/Auth/login', {
        userEmail:email,
        password:password
      })
        .then((response) => {
          localStorage.setItem('token', `${response.data}`)
          setLoginToken(localStorage.getItem("token"));
        })
        .catch((error) => {
          console.log(error);
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
            <button id={styles.button} onClick={buttonClick}>Login</button>
          </div>
        </div>
      </>
    );
}

