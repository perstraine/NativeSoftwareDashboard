import axios from 'axios';
import { useEffect, useState } from 'react'

function Dashboard() {

const [zen, setZen] = useState([]);

//useEffect(() => {
  axios.get('https://localhost:7001/api/Zendesk').then((response) => {
    setZen(response.data.tickets);
    console.log(zen[2].url);
  })
  .catch((error) => {
    console.log(error);
  });
//}, []);

return (
  <div>Dashboard</div>
)
}

export default Dashboard