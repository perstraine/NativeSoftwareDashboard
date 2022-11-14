import styles from "./SupportTicket.module.css"
import {useState} from "react";

function SupportTicket(props) {
  const [isOpen, setIsOpen] = useState(false);
  function handleClick() {
    setIsOpen(!isOpen)
  }
  return (
    <div status-bg-colour={props.ticket.trafficLight} className={styles.ticket}>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.org}> {props.ticket.organisation ? props.ticket.organisation :"Null" }</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.sub}> {props.ticket.subject}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.status}> {props.ticket.status}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.assign}> {props.ticket.assigned ? props.ticket.assigned : "Not assigned"}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.bill}> {props.ticket.billable ? "Yes" : "No"}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.priority}> {props.ticket.priority}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.reqtime}> {props.ticket.requestedDate}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.due}> {props.ticket.timeDue}</p>
      <p status-text-colour={props.ticket.trafficLight} className={styles.ticketText} id={styles.type}> {props.ticket.type ? props.ticket.type :"Null" }</p>
      <div className = {styles.externalLinks} id={styles.links} onClick={handleClick}>
        <div status-dot-colour={props.ticket.trafficLight} className = {styles.dot}></div>
        <div status-dot-colour={props.ticket.trafficLight} className = {styles.dot}></div>
        <div status-dot-colour={props.ticket.trafficLight} className = {styles.dot}></div>
        {isOpen && <div id={styles.dropdown}><a href={props.ticket.url} target="_blank" className={styles.dropdownItem}>View in Zendesk</a></div>}
      </div>
    </div>
  )
}

export default SupportTicket