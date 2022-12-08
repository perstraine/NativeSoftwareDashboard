import axios from 'axios';
import {useState} from "react";
import styles from "../popupWindows/PopupWindows.module.css";


function AddJiraComment(props) {
  const BASE_URL = window.BASE_URL;
    const [name, setName] = useState('');
  const [comment, setComment] = useState('');
  const [disableButton, setDisableButton] = useState(false);
    function handleChange(e) {
        if (e.target.name === "name") {
            setName(e.target.value);
        } else {
            setComment(e.target.value);
        }
    }
  async function submitComment() {
      setDisableButton(true)
      try {
          const url = BASE_URL + "/api/Jira/comment";
            await axios.post(url, {
                name: name,
                message: comment,
                key: props.issue,
            });
            window.alert("Comment added Successfully");
        } catch {
            window.alert("Failed to add Comment");
        } finally {
            setName('');
            setComment('');
        props.setTrigger(false);
        setDisableButton(false);
        }
    }
    return props.trigger ? (
      <div id={styles.popup}>
        <div id={styles.popupinner}>
          <div id={styles.userName}>TechSolutions</div>
          <div id={styles.windowName}>Add Comment</div>
          <form id={styles.formStyle}>
            <div id={styles.subject}>
              <label className={styles.label} style={{ textAlign: "left" }}>
                Name:
              </label>
              <input
                className={styles.input}
                type="name"
                name="name"
                value={name}
                onChange={handleChange}
              ></input>
            </div>
            <div id={styles.comment}>
              <label className={styles.label} style={{ textAlign: "left" }}>
                Comment:
              </label>
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
            <button
              id={styles.closeButton}
              onClick={() => {
                if (!disableButton) submitComment();
              }}
            >
              Ok
            </button>
          </div>
        </div>
      </div>
    ) : (
      ""
    );
}

export default AddJiraComment


