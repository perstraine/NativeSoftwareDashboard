import axios from 'axios';
import {useState} from "react";
import styles from "./PopupWindows.module.css";


function AddJiraRequest(props) {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [subject, setSubject] = useState("");
  const [comment, setComment] = useState("");
  function handleChange(e) {
    switch (e.target.name) {
        case "name":
            setName(e.target.value);
        break;
        case "email":
            setEmail(e.target.value);
        break;
        case "subject":
            setSubject(e.target.value);
        break;
        default:
            setComment(e.target.value);
        break;
    }
  }
  async function submitComment() {
    try {
      await axios.post("https://localhost:7001/api/Jira/request", {
        name: name,
        subject: subject,
          email: email,
        message: comment
      });
      window.alert("Request Sent Successfully");
    } catch {
      window.alert("Failed to Send Request");
    } finally {
        setName("");
        setEmail("");
        setSubject("");
      setComment("");
      props.setTrigger(false);
    }
  }
  return props.trigger ? (
    <div id={styles.popup}>
      <div id={styles.popupinner}>
        <div id={styles.userName}>TechSolutions</div>
        <div id={styles.windowName}>New Jira Request</div>
        <form id={styles.formStyle}>
          <div id={styles.subject}>
            <label className={styles.label}>Name:</label>
            <input
              className={styles.input}
              type="name"
              name="name"
              value={name}
              onChange={handleChange}
            ></input>
          </div>
          <div id={styles.subject}>
            <label className={styles.label}>Your Email:</label>
            <input
              className={styles.input}
              type="name"
              name="email"
              value={email}
              onChange={handleChange}
            ></input>
          </div>
          <div id={styles.subject}>
            <label className={styles.label}>Subjetc:</label>
            <input
              className={styles.input}
              type="name"
              name="subject"
              value={subject}
              onChange={handleChange}
            ></input>
          </div>
          <div id={styles.comment}>
            <label className={styles.label}>Comment:</label>
            <textarea
              id={styles.textArea}
              className={styles.input}
              name="comment"
              value={comment}
              onChange={handleChange}
            ></textarea>
          </div>
        </form>
        <div id={styles.submitbuttons}>
          <button
            id={styles.closeButton}
            onClick={() => props.setTrigger(false)}
          >
            Cancel
          </button>
          <button id={styles.closeButton} onClick={submitComment}>
            Ok
          </button>
        </div>
      </div>
    </div>
  ) : (
    ""
  );
}

export default AddJiraRequest


