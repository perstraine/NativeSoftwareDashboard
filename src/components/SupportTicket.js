import React from 'react'
import styles from "./SupportTicket.module.css"

function SupportTicket(props) {
  //console.log(props.ticket);
  return (
    <div status-bg-colour={props.ticket.priority} className={styles.ticket}>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.org}> {props.ticket.organisation ? props.ticket.organisation :"Null" }</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.sub}> {props.ticket.subject}</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.assign}> {props.ticket.assigned ? props.ticket.assigned : "Not assigned"}</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.bill}> {props.ticket.billable ? "Yes" : "No"}</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.priority}> {props.ticket.priority}</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.reqtime}> {props.ticket.requestedTime}</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.due}> {props.ticket.timeDue}</p>
      <p status-text-colour={props.ticket.priority} className={styles.ticketText} id={styles.type}> {props.ticket.type ? props.ticket.type :"Null" }</p>
      <div className = {styles.externalLinks} id={styles.links}>
        <div status-dot-colour={props.ticket.priority} className = {styles.dot}></div>
        <div status-dot-colour={props.ticket.priority} className = {styles.dot}></div>
        <div status-dot-colour={props.ticket.priority} className = {styles.dot}></div>
      </div>
    </div>
  )
}

export default SupportTicket