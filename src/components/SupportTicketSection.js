import React from 'react'
import axios from 'axios';
import { useEffect, useState } from 'react'
import styles from "./SupportTicket.module.css"
import SupportTicket from './SupportTicket'

function SupportTicketSection() {
  const [ticketList, setTicketList] = useState();
  //const [TicketElement, setSupportTicketElement] = useState();

  //Zendesk API
  useEffect(() => {
    axios.get('https://localhost:7001/api/Zendesk').then((response) => {
      if (ticketList) {
        console.log('data already retrieved')
        console.log(ticketList);
      }else
      {setTicketList(response.data);
      console.log(ticketList);}
    })
    .catch((error) => {
      console.log(error);
    });
  }, [ticketList]);

  //if(ticketList){
    const TicketElement = ticketList.map((element) =>{
      return <SupportTicket key={element.id} ticket={element}/>;
    });
  //}
  
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
      <SupportTicket/>
      <SupportTicket/>
      <SupportTicket/>
    </div>
  )
}

export default SupportTicketSection