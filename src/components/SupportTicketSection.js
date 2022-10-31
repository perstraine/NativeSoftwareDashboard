import React from 'react'
import axios from 'axios';
import { useEffect, useState } from 'react'
import styles from "./SupportTicket.module.css"
import SupportTicket from './SupportTicket'

function SupportTicketSection() {
  const [ticketList, setTicketList] = useState([] );

  //Zendesk API
  useEffect(() => {
    getTicket();
  },[]);

  async function getTicket() {
      try {
        const response = await axios.get('https://localhost:7001/api/Zendesk')
        setTicketList(response.data);
        //console.log(response.data);
      }
      catch (error) {
        console.error(error);
      }
  }

  const TicketElement = ticketList.map((element) =>{
    return <SupportTicket key={element.id} ticket={element}/>;
  });
  
  return (
    <div id={styles.supportticketsection}>
      <div id={styles.ticketheadings}>
          <p> Organisation</p>
          <p> Subject</p>
          <p> Assigned</p>
          <p> Billable</p>
          <p> Priority</p>
          <p> Requested Time</p>
          <p> Time Due</p>
          <p> Type</p>
      </div>
      {TicketElement}
    </div>
  )
}

export default SupportTicketSection