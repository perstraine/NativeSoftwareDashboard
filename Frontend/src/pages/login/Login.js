import axios from 'axios';
import styles from "./Login.module.css"
import Logo from './assets/Logo.png'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import FadeLoader  from "react-spinners/FadeLoader";

export default function Login() {
  const BASE_URL = window.BASE_URL;
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [logintoken, setLoginToken] = useState('');
  const [errorMessage, setErrorMessage] = useState('');
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);

  useEffect(() =>{
    setLoading(true);
    setTimeout(() => {
      setLoading(false)
    })
  },[])

  useEffect(() => {
    setLoginToken(localStorage.getItem("token"));
    if(logintoken)
    {
      const config = { headers: { Authorization: `Bearer ${logintoken}` } };
      const url = BASE_URL + "/Auth/login";
      axios.get(url,config)
        .then((response) => {
          console.log(response);
          if (response.data === 'User Authorised') {
            if (localStorage.getItem("userType") === "Staff") {
              navigate("/dashboard");
            } else{
              navigate("/customerdashboard");
            }
          }
          else{
            setErrorMessage("Email and password does not match. Please try again.")
          }
        })
      .catch((err) =>{

      })}
  }, [logintoken, navigate])

  function handleChange(e) {
      if (e.target.name === 'email') {
          setEmail(e.target.value)
      } else {
          setPassword(e.target.value)
      }
  }

  const buttonClick = (e) => {
      setLoading(!loading);
      e.preventDefault();
      setEmail(email);
      localStorage.setItem('email',email);
      setPassword(password);
      const url = BASE_URL + "/Auth/login";
      axios.post(url, {
        userEmail:email,
        password:password
      })
        .then((response) => {
          localStorage.setItem('token', `${response.data.token}`)
          localStorage.setItem('userType',`${response.data.userType}`)
          setLoginToken(localStorage.getItem("token"));

          if(localStorage.getItem('userType') === 'Staff'){
            navigate('/dashboard');
          }
          else{
            navigate('/customerdashboard');
          }
          setLoading(false);
        })
        .catch((error) => {
          console.log(error);
            setErrorMessage(
              "Email and password does not match. Please try again."
            );
          setLoading(false);
        });
    };

    return (
      <>
        {loading ? (
          <div className={styles.loader}>
            <FadeLoader color="#7b73ff" height={17} margin={6} width={4} />
          </div>
        ) : (
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
              <button id={styles.button} onClick={buttonClick}>
                Login
              </button>
              <div>{errorMessage}</div>
            </div>
          </div>
        )}
      </>
    );
}

