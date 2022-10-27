import axios from 'axios';
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";

function Dashboard() {

  const [jira, setJira] = useState('');
  const [zendesk, setZendesk] = useState('');
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

//Zendesk API
useEffect(() => {
  axios.get('https://localhost:7001/api/Zendesk').then((response) => {
    if (zendesk) {
      console.log('data already retrieved')
      console.log(zendesk);
    }else
    {setZendesk(response.data);
    console.log(zendesk);}
  })
  .catch((error) => {
    console.log(error);
  });
}, [zendesk]);
  
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
  <div>
    <div>Dashboard</div>
    <button onClick={logout}>Log Out</button>
  </div>
);
}

export default Dashboard