import axios from 'axios';
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";

function Dashboard() {

  const [jira, setJira] = useState('');
  const navigate = useNavigate();

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
        .get("https://localhost:7001/api/Users", config);
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
    <div onClick={logout}>Log Out</div>
  </div>
);
}

export default Dashboard