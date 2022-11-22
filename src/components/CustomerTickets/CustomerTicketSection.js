import React from 'react'
import axios from 'axios';
import { useEffect, useState } from 'react'
import styles from "./CustomerTicket.module.css"
import CustomerTicket from './CustomerTicket';
import FadeLoader  from "react-spinners/FadeLoader";

function CustomerTicketSection() {
    const [loading, setLoading] = useState(true);
    const [ticketList, setTicketList] = useState([]);
  
    useEffect(() =>{
      setLoading(false);
      setTimeout(() => {
        setLoading(true)
      })
    },[])
    
    //Zendesk API
    useEffect(() => {
        getTicket();
    },[]);
  
    async function getTicket() {
      try {
        let userType = localStorage.getItem('userType');
        //const response = await axios.get('https://localhost:7001/api/CustomerView');
        const response = await axios.get('https://localhost:7001/api/CustomerView', { params: { userType: userType } });
        setTicketList(response.data);
        setLoading(!loading);
      }
      catch (error) {
        console.error(error);
      }
    }
  
    const TicketElements = ticketList.map((element) =>{
      return <CustomerTicket key={element.id} ticket={element}/>;
    });
  return (
    <div id={styles.customerticketsection}>
      <div id={styles.ticketheadings}>
          <p id={styles.sub}> Subject</p>
          <p id={styles.reqTime}> Requested Date</p>
          <p id={styles.firstRes}> First Response</p>
          <p id={styles.lastUpdate}> Last Update</p>
          <p id={styles.due}> Time Due</p>
          <p id={styles.priority}> Priority</p>
          <p id={styles.type}> Type</p>
          <div id = {styles.links}></div>
      </div>

      { 
      loading?
      <FadeLoader
        color="#81E8FF"
        height={17}
        margin={6}
        width={4}
      /> :
      TicketElements
      } 

      <div>
        <div id={styles.chevronArrowDown}></div>
      </div>
    </div>
  )
}

export default CustomerTicketSection