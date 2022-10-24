import axios from 'axios';
import styles from "./Login.module.css"
import Logo from './assets/Logo.png'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';


export default function Login() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [logintoken, setLoginToken] = useState('')
    const navigate = useNavigate();

  useEffect(() => {
  if(logintoken)
    {const config = { headers: { Authorization: `Bearer ${logintoken}` } };
    axios.get("https://localhost:7001/api/Users",config)
      .then((response) => {
        console.log(response);
        if (response.data == 'User Authorised') {
          navigate('/dashboard');
          console.log('User Authorised');
        }
      })}
  }, [logintoken])

  // function loginCheck(logintoken) {
  //   const config = { headers: { Authorization: `Bearer ${logintoken}` } }
  //   console.log('inside longin check function', logintoken)
  //   axios.get("https://localhost:7001/api/Users",config)
  //     .then((response) => {
  //       console.log(response.data);
  //     } )}

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
      //console.log(`userEmail: ${email}, pass: ${password}`)
      axios.post('https://localhost:7001/Auth/login', {
        userEmail:email,
        password:password
      })
        .then((response) => {
          setLoginToken(response.data);
          // loginCheck(logintoken);
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
            <div>{logintoken}</div>
          </div>
        </div>
      </>
    );
}

