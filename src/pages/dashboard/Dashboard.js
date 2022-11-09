import axios from 'axios';
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import SupportTicketSection from '../../components/SupportTicketSection';
import styles from "./Dashboard.module.css"

function Dashboard() {

  const [jira, setJira] = useState('');
  const navigate = useNavigate();

// Jira API
useEffect(() => {
  axios.get('https://localhost:7001/api/Jira').then((response) => {
    if (jira) {
      console.log('data already retrieved')
      console.log(jira);
    }else
    {setJira(response.data.issues);
    console.log(jira);}
  })
  .catch((error) => {
    console.log(error);
  });
}, [jira]);
  
  useEffect(() => {
  const logintoken = localStorage.getItem('token')
    const config = { headers: { Authorization: `Bearer ${logintoken}` } };
    if (logintoken)
    {try {
      axios
        .get("https://localhost:7001/Auth/login", config);
    } catch (error) {
      navigate('/');
      }
    } else {
      navigate("/");
    }
  })

  function logout() {
    localStorage.removeItem('token')
    navigate('/')
  }
return (
  <div id={styles.dashboard}>
    <h1>Dashboard</h1>
    <SupportTicketSection/>
    <button onClick={logout}>Log Out</button>
  </div>
);
}

export default Dashboard