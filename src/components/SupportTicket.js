import React from 'react'
import styles from "./SupportTicket.module.css"

function SupportTicket(props) {
  //console.log(props.ticket);
  return (
    <div id={styles.ticket}>
      <p> {props.ticket.organisation ? props.ticket.organisation :"Null" }</p>
      <p> {props.ticket.subject}</p>
      <p> {props.ticket.assigned ? props.ticket.assigned : "Not assigned"}</p>
      <p> {props.ticket.billable ? "Yes" : "No"}</p>
      <p> {props.ticket.priority}</p>
      <p> {props.ticket.requestedTime}</p>
      <p> {props.ticket.timeDue}</p>
      <p> {props.ticket.type}</p>
    </div>
  )
}

export default SupportTicket