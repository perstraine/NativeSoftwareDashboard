import React from 'react'
import styles from "./SupportTicket.module.css"

function SupportTicket(props) {
  return (
    <div id={styles.ticket}>
    <p> Organisation</p>
    <p> {props.ticket.Subject}</p>
    <p> Subject</p>
    <p> Assigned</p>
    <p> Billable</p>
    <p> Priority</p>
    <p> Requested Time</p>
    <p> Time Due</p>
    <p> Type</p>
</div>
  )
}

export default SupportTicket