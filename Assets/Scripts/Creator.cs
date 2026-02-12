using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Creator
{
  public class Page
  {
    public int key = 0;
    public string name = string.Empty;
    public List<Element> elements = new();
    public Page()
    {
      key = Guid.NewGuid().GetHashCode();
    }
  }
  [JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
  [JsonDerivedType(typeof(GENERAL_TEXT), typeDiscriminator: "Element.GENERAL_TEXT")]
  [JsonDerivedType(typeof(GENERAL_CODE_COMMAND), typeDiscriminator: "Element.GENERAL_CODE_COMMAND")]
  [JsonDerivedType(typeof(GENERAL_CODE_SCRIPT), typeDiscriminator: "Element.GENERAL_CODE_SCRIPT")]
  [JsonDerivedType(typeof(GENERAL_LINE), typeDiscriminator: "Element.GENERAL_LINE")]
  [JsonDerivedType(typeof(GENERAL_WARNING), typeDiscriminator: "Element.GENERAL_WARNING")]
  [JsonDerivedType(typeof(SYSTEM_QUIZ_ANSWERS), typeDiscriminator: "Element.SYSTEM_QUIZ_ANSWERS")]
  [JsonDerivedType(typeof(SYSTEM_QUIZ_RESULT), typeDiscriminator: "Element.SYSTEM_QUIZ_RESULT")]
  [JsonDerivedType(typeof(SYSTEM_QUIZZES_MENU), typeDiscriminator: "Element.SYSTEM_QUIZ_MENU")]
  [JsonDerivedType(typeof(CHALLENGE_MULTIPLE_CHOICE), typeDiscriminator: "Element.CHALLENGE_MULTIPLE_CHOICE")]
  [JsonDerivedType(typeof(CHALLENGE_INPUT_NORMAL), typeDiscriminator: "Element.CHALLENGE_INPUT_NORMAL")]
  public class Element
  {
    public int key = 0;
    public TYPE type = TYPE.EMPTY;
    public Element()
    {
      key = Guid.NewGuid().GetHashCode();
    }
    public virtual void Insert_In_Page(Transform trfm_inst) { }
    public virtual string To_Text() { return string.Empty; }
    public static string Name(TYPE type)
    {
      return type switch
      {
        TYPE.EMPTY => throw new NotImplementedException(),
        TYPE.GENERAL_TEXT => "Text",
        TYPE.GENERAL_CODE_COMMAND => "Code Command",
        TYPE.GENERAL_LINE => "Line",
        TYPE.GENERAL_IMAGE => "Image",
        TYPE.GENERAL_TTS => "Text To Speech",
        TYPE.GENERAL_CODE_SCRIPT => "Code Script",
        TYPE.CHALLENGE_MULTIPLE_CHOICE => "Multiple Choice",
        TYPE.CHALLENGE_INPUT_NORMAL => "Input Normal",
        TYPE.GENERAL_WARNING => "Warning",
        TYPE.AI_PROMPT => "AI Prompt",
        TYPE.SYSTEM_QUIZ_RESULT => string.Empty,
        TYPE.SYSTEM_QUIZ_ANSWERS => string.Empty,
        _ => string.Empty
      };
    }
    public static string ToolTip(TYPE type)
    {
      return type switch
      {
        TYPE.EMPTY => "Placeholder or undefined content.",
        TYPE.GENERAL_TEXT => "Standard textual content.",
        TYPE.GENERAL_CODE_COMMAND => "Code block meant for commands or terminal input.",
        TYPE.GENERAL_LINE => "Divider or separator line.",
        TYPE.GENERAL_IMAGE => "Embedded image content.",
        TYPE.GENERAL_CODE_SCRIPT => "Code block for scripting languages.",
        TYPE.CHALLENGE_MULTIPLE_CHOICE => "A multiple-choice quiz.",
        TYPE.CHALLENGE_INPUT_NORMAL => "A challenge requiring typed user input.",
        TYPE.GENERAL_WARNING => "Warning message or alert.",
        TYPE.AI_PROMPT => "Content intended for AI-generated interaction by prompt input.",
        TYPE.SYSTEM_QUIZ_RESULT => "Output displaying quiz or challenge results.",
        TYPE.GENERAL_TTS => "Text-to-speech content.",
        _ => "Unknown type."
      };
    }
    public enum TYPE
    {
      EMPTY = 0,
      GENERAL_TEXT = -428893296,
      GENERAL_CODE_COMMAND = 813157805,
      GENERAL_CODE_SCRIPT = 1254542888,
      GENERAL_LINE = 1267293381,
      GENERAL_WARNING = 1255947363,
      GENERAL_IMAGE = 1836690311,
      GENERAL_TTS = 227252196,
      CHALLENGE_MULTIPLE_CHOICE = 181698377,
      CHALLENGE_INPUT_NORMAL = -1606431612,
      AI_PROMPT = 51721585,
      SYSTEM_QUIZ_RESULT = -1479532188,
      SYSTEM_QUIZ_ANSWERS = 128397823,
      SYSTEM_QUIZZES_MENU = -238138132,
    }
    public enum TYPE_GENERAL
    {
      EMPTY = 0,
      TEXT = TYPE.GENERAL_TEXT,
      CODE_COMMAND = TYPE.GENERAL_CODE_COMMAND,
      CODE_SCRIPT = TYPE.GENERAL_CODE_SCRIPT,
      LINE = TYPE.GENERAL_LINE,
      IMAGE = TYPE.GENERAL_IMAGE,
      WARNING = TYPE.GENERAL_WARNING,
      TEXT_TO_SPEECH = TYPE.GENERAL_TTS,
    }
    public enum TYPE_CHALLENGE
    {
      EMPTY = 0,
      MULTIPLE_CHOICE = TYPE.CHALLENGE_MULTIPLE_CHOICE,
      INPUT_NORMAL = TYPE.CHALLENGE_INPUT_NORMAL,
    }
    public enum TYPE_AI
    {
      EMPTY = 0,
      PROMPT = TYPE.AI_PROMPT,
    }
    public class GENERAL_TEXT : Element
    {
      public GENERAL_TEXT()
      {
        type = TYPE.GENERAL_TEXT;
      }
      public SPACING spacing = new()
      {
        left = 15,
        right = 15,
        top = 6,
        bottom = 6,
      };
      public ALIGNMENT_HORIZONTAL alignment_horizontal = ALIGNMENT_HORIZONTAL.LEFT;
      public string color_hex = "#000000";
      public bool bold_enabled = false;
      public bool italic_enabled = false;
      public bool underline_enabled = false;
      public int font_size = 18;
      public string text_content = "Text element...";
      public override string To_Text()
      {
        return text_content;
      }
      public override void Insert_In_Page(Transform trfm_inst)
      {
        var text_field = trfm_inst.Find("text").GetComponent<TMP_Text>();
        // alignment horizontal
        {
          switch (alignment_horizontal)
          {
            case ALIGNMENT_HORIZONTAL.LEFT:
              text_field.horizontalAlignment = HorizontalAlignmentOptions.Left;
              break;
            case ALIGNMENT_HORIZONTAL.CENTER:
              text_field.horizontalAlignment = HorizontalAlignmentOptions.Center;
              break;
            case ALIGNMENT_HORIZONTAL.RIGHT:
              text_field.horizontalAlignment = HorizontalAlignmentOptions.Right;
              break;
          }
        }
        // margin
        {
          var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
          layout.padding.left = spacing.left;
          layout.padding.right = spacing.right;
          layout.padding.top = spacing.top;
          layout.padding.bottom = spacing.bottom;
        }
        // color
        {
          ColorUtility.TryParseHtmlString(color_hex, out var color);
          text_field.color = color;
        }
        // font size
        {
          text_field.fontSize = font_size;
        }
        // text content
        {
          text_field.text = text_content;
        }
        // style
        {
          var style = FontStyles.Normal;
          if (bold_enabled)
            style |= FontStyles.Bold;
          if (italic_enabled)
            style |= FontStyles.Italic;
          if (underline_enabled)
            style |= FontStyles.Underline;
          text_field.fontStyle = style;
        }
      }
    }
    public class GENERAL_WARNING : Element
    {
      public GENERAL_WARNING()
      {
        type = TYPE.GENERAL_WARNING;
      }
      public SPACING spacing = new()
      {
        left = 20,
        right = 20,
        top = 6,
        bottom = 6,
      };
      public string color_hex_frame = "#E5D4C7";
      public string color_hex_text = "#C13B0C";
      public string text_content = "This is a preview of the warning element.";
      public override string To_Text()
      {
        return $"Warning: {text_content}";
      }
      public override void Insert_In_Page(Transform trfm_inst)
      {
        // margin
        {
          var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
          layout.padding.left = spacing.left;
          layout.padding.right = spacing.right;
          layout.padding.top = spacing.top;
          layout.padding.bottom = spacing.bottom;
        }
        // text content
        {
          trfm_inst.Find("layout/text").GetComponent<TMP_Text>().text = text_content;
        }
        // color frame
        {
          ColorUtility.TryParseHtmlString(color_hex_frame, out var color);
          trfm_inst.Find("layout/frame").GetComponent<Image>().color = color;
        }
        // color text
        {
          ColorUtility.TryParseHtmlString(color_hex_text, out var color);
          trfm_inst.Find("layout/text").GetComponent<TMP_Text>().color = color;
          trfm_inst.Find("layout/text/icon").GetComponent<Image>().color = color;
        }
      }
    }
    public class GENERAL_CODE_COMMAND : Element
    {
      public GENERAL_CODE_COMMAND()
      {
        type = TYPE.GENERAL_CODE_COMMAND;
      }
      public SPACING spacing = new()
      {
        left = 6,
        right = 6,
        top = 6,
        bottom = 6,
      };
      public string text_content = "";
      public bool lines_enabled = false;
      public override string To_Text()
      {
        return
        $"```\n"
        + $"{text_content}"
        + $"\n```";
      }
      public override void Insert_In_Page(Transform trfm_inst)
      {
        var text_field = trfm_inst.Find("layout/content/text").GetComponent<TMP_Text>();
        // margin
        {
          var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
          layout.padding.left = spacing.left;
          layout.padding.right = spacing.right;
          layout.padding.top = spacing.top;
          layout.padding.bottom = spacing.bottom;
        }
        // text content
        {
          if (lines_enabled)
            text_field.text = add_line_numbers(text_content);
          else
            text_field.text = text_content;
        }
        string add_line_numbers(string texto)
        {
          var lines = texto.Replace("\r\n", "\n").Split('\n');
          var result = new System.Text.StringBuilder();
          for (int i = 0; i < lines.Length; i++)
          {
            string num = $"<color=#A6A6A6>{i + 1,3}</color>: ";
            result.AppendLine(num + lines[i]);
          }
          return result.ToString();
        }
      }
    }
    public class GENERAL_LINE : Element
    {
      public GENERAL_LINE()
      {
        type = TYPE.GENERAL_LINE;
      }
      public SPACING spacing = new()
      {
        left = 15,
        right = 15,
        top = 6,
        bottom = 6,
      };
      public string color_hex = "#F2F2F2";
      public float height = 3;
      public override void Insert_In_Page(Transform trfm_inst)
      {
        // margin
        {
          var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
          layout.padding.left = spacing.left;
          layout.padding.right = spacing.right;
          layout.padding.top = spacing.top;
          layout.padding.bottom = spacing.bottom;
        }
        // height
        {
          trfm_inst.Find("line").GetComponent<LayoutElement>().preferredHeight = height;
        }
        // color
        {
          ColorUtility.TryParseHtmlString(color_hex, out var color);
          trfm_inst.Find("line").GetComponent<Image>().color = color;
        }
      }
    }
    public class GENERAL_CODE_SCRIPT : Element
    {
      public GENERAL_CODE_SCRIPT()
      {
        type = TYPE.GENERAL_CODE_SCRIPT;
      }
      public SPACING spacing = new()
      {
        left = 6,
        right = 6,
        top = 6,
        bottom = 6,
      };
      public string text_content = "";
      public string text_title = "";
      public bool lines_enabled = false;
      public override string To_Text()
      {
        return
        $"{text_title}\n"
        + $"```\n"
        + $"{text_content}"
        + $"\n```";
      }
      public override void Insert_In_Page(Transform trfm_inst)
      {
        var text_field = trfm_inst.Find("layout/content/text").GetComponent<TMP_Text>();
        // margin
        {
          var layout = trfm_inst.GetComponent<HorizontalOrVerticalLayoutGroup>();
          layout.padding.left = spacing.left;
          layout.padding.right = spacing.right;
          layout.padding.top = spacing.top;
          layout.padding.bottom = spacing.bottom;
        }
        // title
        {
          trfm_inst.Find("layout/title/text").GetComponent<TMP_Text>().text = text_title;
        }
        // text content
        {
          if (lines_enabled)
            text_field.text = add_line_numbers(text_content);
          else
            text_field.text = text_content;
        }
        string add_line_numbers(string texto)
        {
          var lines = texto.Replace("\r\n", "\n").Split('\n');
          var result = new System.Text.StringBuilder();
          for (int i = 0; i < lines.Length; i++)
          {
            string num = $"<color=#A6A6A6>{i + 1,3}</color>: ";
            result.AppendLine(num + lines[i]);
          }
          return result.ToString();
        }
      }
    }
    public class CHALLENGE_MULTIPLE_CHOICE : Element
    {
      public CHALLENGE_MULTIPLE_CHOICE()
      {
        type = TYPE.CHALLENGE_MULTIPLE_CHOICE;
        options.list.Add(new() { text = "Option 1" });
      }
      [JsonIgnore] public ANSWER answer = null;
      public int submit_points = 0;
      public QUESTION question = new();
      public OPTIONS options = new();
      public bool displayECAs = true;
      public bool Verbosity = true;
      public class QUESTION
      {
        public ALIGNMENT_HORIZONTAL alignment_horizontal = ALIGNMENT_HORIZONTAL.CENTER;
        public string color_hex = "#000000";
        public bool bold_enabled = false;
        public bool italic_enabled = false;
        public bool underline_enabled = false;
        public int font_size = 20;
        public string text_content = "Question Here...";
        public SPACING spacing = new()
        {
          left = 150,
          right = 150,
          top = 30,
          bottom = 15,
        };
      }
      public class OPTIONS
      {
        public bool shuffle_enabled = false;
        public int option_correct_key = 0;
        public List<OPTION> list = new();
        public SPACING spacing = new()
        {
          left = 190,
          right = 190,
          top = 10,
          bottom = 10,
        };
      }
      public class OPTION
      {
        public int key = 0;
        public string text = string.Empty;
        public OPTION()
        {
          key = Guid.NewGuid().GetHashCode();
        }
      }
      public class ANSWER
      {
        public int option_submit_key = 0;
      }
      public override string To_Text()
      {
        var lines = new List<string> { "Quiz Multiple Choice" };
        // question
        {
          lines.Add("Question:");
          lines.Add($"{question.text_content}");
        }
        // options
        {
          lines.Add("Options:");
          foreach (var option in options.list)
            lines.Add($"{options.list.IndexOf(option) + 1}) {option.text}");
        }
        // answers
        {
          lines.Add("Correct Answer:");
          lines.Add(
            options.list.Find(m => m.key == options.option_correct_key)?.text
            ?? "Empty");
          lines.Add("User Answer:");
          lines.Add(
            @$"{(
              answer != null
              ? options.list.Find(m => m.key == answer.option_submit_key)?.text
              ?? "Empty"
              : "Empty")}"
          );
        }
        return string.Join("\n", lines.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
      public string To_Text_WithOutHelper()
      {
        var lines = new List<string> { "Quiz Multiple Choice" };
        // question
        {
          lines.Add("Question:");
          lines.Add($"{question.text_content}");
        }
        // options
        {
          lines.Add("Options:");
          foreach (var option in options.list)
            lines.Add($"{options.list.IndexOf(option) + 1}) {option.text}");
        }
        return string.Join("\n", lines.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
      public override void Insert_In_Page(Transform trfm_inst)
      {
        // question
        {
          var text_field = trfm_inst.Find("question/text").GetComponent<TMP_Text>();
          // alignment horizontal
          {
            switch (question.alignment_horizontal)
            {
              case ALIGNMENT_HORIZONTAL.LEFT:
                text_field.horizontalAlignment = HorizontalAlignmentOptions.Left;
                break;
              case ALIGNMENT_HORIZONTAL.CENTER:
                text_field.horizontalAlignment = HorizontalAlignmentOptions.Center;
                break;
              case ALIGNMENT_HORIZONTAL.RIGHT:
                text_field.horizontalAlignment = HorizontalAlignmentOptions.Right;
                break;
            }
          }
          // margin
          {
            var layout = trfm_inst.Find("question").GetComponent<HorizontalOrVerticalLayoutGroup>();
            layout.padding.left = question.spacing.left;
            layout.padding.right = question.spacing.right;
            layout.padding.top = question.spacing.top;
            layout.padding.bottom = question.spacing.bottom;
          }
          // color
          {
            ColorUtility.TryParseHtmlString(question.color_hex, out var color);
            text_field.color = color;
          }
          // font size
          {
            text_field.fontSize = question.font_size;
          }
          // text content
          {
            text_field.text = question.text_content;
          }
          // style
          {
            var style = FontStyles.Normal;
            if (question.bold_enabled)
              style |= FontStyles.Bold;
            if (question.italic_enabled)
              style |= FontStyles.Italic;
            if (question.underline_enabled)
              style |= FontStyles.Underline;
            text_field.fontStyle = style;
          }
        }
        // options
        {
          // margin
          {
            var layout = trfm_inst.Find("options").GetComponent<HorizontalOrVerticalLayoutGroup>();
            layout.padding.left = options.spacing.left;
            layout.padding.right = options.spacing.right;
            layout.padding.top = options.spacing.top;
            layout.padding.bottom = options.spacing.bottom;
          }
          // list
          {
            var options_parent = trfm_inst.Find("options");
            options_parent.GetChild(0).gameObject.SetActive(false);
            for (int i = 1; i < options_parent.childCount; i++) GameObject.Destroy(options_parent.GetChild(i).gameObject);
            foreach (var option in options.list)
            {
              var trfm_option = GameObject.Instantiate(options_parent.GetChild(0), options_parent);
              trfm_option.name = option.key.ToString();
              trfm_option.gameObject.SetActive(true);
              trfm_option.Find("text").GetComponent<TMP_Text>().text = option.text;
            }
          }
        }
      }
    }
    public class CHALLENGE_INPUT_NORMAL : Element
    {
      public CHALLENGE_INPUT_NORMAL()
      {
        type = TYPE.CHALLENGE_INPUT_NORMAL;
      }
      public int submit_points = 0;
      public QUESTION question = new();
      public INPUT_FIELD input_field = new();
      public bool displayECAs = true;
      public bool Verbosity = true;

      public class QUESTION
      {
        public ALIGNMENT_HORIZONTAL alignment_horizontal = ALIGNMENT_HORIZONTAL.CENTER;
        public string color_hex = "#000000";
        public bool bold_enabled = false;
        public bool italic_enabled = false;
        public bool underline_enabled = false;
        public int font_size = 20;
        public string text_content = "Question Here...";
        public SPACING spacing = new()
        {
          left = 15,
          right = 15,
          top = 6,
          bottom = 6,
        };
      }
      public class INPUT_FIELD
      {
        public float height = 150;
        public SPACING spacing = new()
        {
          left = 300,
          right = 300,
          top = 10,
          bottom = 10,
        };
      }
      
      public override string To_Text()
      {
        var lines = new List<string> { "Quiz INPUT_NORMAL" };
        #region question
        {
          lines.Add("Question:");
          lines.Add($"{question.text_content}");
        }
        #endregion
        #region  input field
        {
          lines.Add("Input Field:");
          lines.Add($"{question.text_content}");
        }
        #endregion
        
        return string.Join("\n", lines.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
      public override void Insert_In_Page(Transform trfm_inst)
      {
        
        #region + question
        var text_field = trfm_inst.Find("question/text").GetComponent<TMP_Text>();
        // alignment horizontal
        switch (question.alignment_horizontal)
        {
          case ALIGNMENT_HORIZONTAL.LEFT:
            text_field.horizontalAlignment = HorizontalAlignmentOptions.Left;
            break;
          case ALIGNMENT_HORIZONTAL.CENTER:
            text_field.horizontalAlignment = HorizontalAlignmentOptions.Center;
            break;
          case ALIGNMENT_HORIZONTAL.RIGHT:
            text_field.horizontalAlignment = HorizontalAlignmentOptions.Right;
            break;
        }
        
        // margin
        var layout = trfm_inst.Find("question").GetComponent<HorizontalOrVerticalLayoutGroup>();
        layout.padding.left = question.spacing.left;
        layout.padding.right = question.spacing.right;
        layout.padding.top = question.spacing.top;
        layout.padding.bottom = question.spacing.bottom;
      
        // color
        ColorUtility.TryParseHtmlString(question.color_hex, out var color);
        text_field.color = color;
      
        // font size
        text_field.fontSize = question.font_size;
        
        // text content
        text_field.text = question.text_content;
        
        // style
        var style = FontStyles.Normal;
        if (question.bold_enabled)
          style |= FontStyles.Bold;
        if (question.italic_enabled)
          style |= FontStyles.Italic;
        if (question.underline_enabled)
          style |= FontStyles.Underline;
        text_field.fontStyle = style;
        #endregion

        #region + input field
        // height
        var field = trfm_inst.Find("input/field").GetComponent<LayoutElement>();
        
        field.preferredHeight = input_field.height;
        
        // spacing
        var input = trfm_inst.Find("input").GetComponent<HorizontalOrVerticalLayoutGroup>();
        input.padding.left = input_field.spacing.left;
        input.padding.right = input_field.spacing.right;
        input.padding.top = input_field.spacing.top;
        input.padding.bottom = input_field.spacing.bottom;
        #endregion
      }
    }
    public class AI_PROMPT
    {
      public AI_MODEL ai_model = AI_MODEL.DEFAULT;
      public bool writter_effect = false;
      public bool page_context = false;
      public bool tts = false;
      public bool submit = false;
      public string message_system = string.Empty;
      public string message_user = string.Empty;
      [JsonIgnore] public string message_assistant = null;
      [JsonIgnore]
      public string context
      {
        get
        {
          var value = string.Empty;
          value =
          $"{message_assistant}";
          return value;
        }
      }
    }
    public class SYSTEM_QUIZ_RESULT : Element
    {
      public SYSTEM_QUIZ_RESULT()
      {
        type = TYPE.SYSTEM_QUIZ_RESULT;
      }
      public override string To_Text()
      {
        var lines = new List<string>
        {
          $"Quiz Result"
        };
        var quiz_result = GameData.Class_Current?.last_quiz_result;
        if (quiz_result != null)
        {
          lines.Add($"- Time: {quiz_result.time_amount} seconds.");
          lines.Add($"- Points: {quiz_result.points_amount} points.");
          lines.Add($"- Answers Corrects: {quiz_result.answers_correct_amount} correct's. of the {quiz_result.answers.Count}");
          lines.Add($"- Answers Percentage Correct: {(quiz_result.answers_correct_amount * 100) / quiz_result.answers.Count}%");
          lines.Add($"- {quiz_result.answers_correct_amount} Correct's");
          lines.Add($"- {quiz_result.answers.Count - quiz_result.answers_correct_amount} Incorrect's");
        }
        return string.Join("\n", lines.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
    }
    public class SYSTEM_QUIZ_ANSWERS : Element
    {
      public SYSTEM_QUIZ_ANSWERS()
      {
        type = TYPE.SYSTEM_QUIZ_ANSWERS;
      }
      public override string To_Text()
      {
        var lines = new List<string>
        {
          $"Quiz Answers"
        };
        var quiz_result = GameData.Class_Current?.last_quiz_result;
        if (quiz_result != null)
          foreach (var answer_base in quiz_result.answers)
            lines.Add(
              answer_base.To_Text()
              .Replace("Quiz Multiple Choice", "")
              .Replace("Question:", $"Question {quiz_result.answers.IndexOf(answer_base) + 1}:"));
        return string.Join("\n", lines.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
    }
    public class SYSTEM_QUIZZES_MENU : Element
    {
      public SYSTEM_QUIZZES_MENU()
      {
        type = TYPE.SYSTEM_QUIZZES_MENU;
      }
      public string title = "Title...";
      public string description = "Description...";
      public override void Insert_In_Page(Transform trfm_inst)
      {
        trfm_inst.Find("title").GetComponent<TMP_Text>().text = title;
        trfm_inst.Find("description").GetComponent<TMP_Text>().text = description;
      }
      public override string To_Text()
      {
        var lines = new List<string>
        {
          $"This is a Quiz Menu",
          $"Title: {title}",
          $"Description: {description}",
          $"Quizzes Tree Categories:",
        };
        if (GameData.Class_Current != null)
          foreach (var quiz in GameData.Class_Current.Quizzes)
            lines.Add($"- {quiz.name}");
        return string.Join("\n", lines.Where(s => !string.IsNullOrWhiteSpace(s)));
      }
    }
    public class GENERAL_TTS
    {
      public bool writter_effect = false;
      public bool submit = false;
      public string text = string.Empty;
      [JsonIgnore]
      public string context
      {
        get
        {
          var value = string.Empty;
          value =
          $"{text}";
          return value;
        }
      }
    }
    public Element Clone()
    {
      var json_options = new JsonSerializerOptions { IncludeFields = true };
      var element_serialized = JsonSerializer.Serialize(this, json_options);
      var element_deserialized = JsonSerializer.Deserialize<Element>(element_serialized, json_options);
      return element_deserialized;
    }
    public void Keys_CopyBy(Element other)
    {
      if (other != null && other.type != this.type) return;
      key = other == null
      ? Guid.NewGuid().GetHashCode()
      : other.key;
    }
    public enum ALIGNMENT_HORIZONTAL
    {
      LEFT,
      CENTER,
      RIGHT
    }
    public class SPACING
    {
      public int left;
      public int right;
      public int top;
      public int bottom;
    }
    public enum AI_MODEL
    {
      EMPTY,
      DEFAULT = 1897049,
      OPEN_AI = -5943048,
      LOCAL = 9132979
    }
  }
  public class Class
  {
    public string version = "2";
    public int key = 0;
    public string name = string.Empty;
    public List<Lesson> Lessons = new();
    public Page Quizzes_Menu = new();
    public List<Quiz> Quizzes = new();
    [JsonIgnore] public Quiz_Result last_quiz_result = null;
    public Class()
    {
      key = Guid.NewGuid().GetHashCode();
      // quiz menu
      {
        Quizzes_Menu = new();
        Quizzes_Menu.elements.Add(new Element.SYSTEM_QUIZZES_MENU());
      }
    }
    public class Quiz
    {
      public int key = 0;
      public string name = string.Empty;
      public List<Page> pages = new();
      public Page page_result = new();
      public Page page_answers = new();
      public Quiz()
      {
        key = Guid.NewGuid().GetHashCode();
        // page result
        {
          page_result = new();
          page_result.elements.Add(new Element.SYSTEM_QUIZ_RESULT());
        }
        // page answers
        {
          page_answers = new();
          page_answers.elements.Add(new Element.SYSTEM_QUIZ_ANSWERS());
        }
      }
    }
    public class Quiz_Result
    {
      public Quiz quiz = null;
      public List<Element> answers = new();
      public int time_amount = 0;
      public int points_amount = 0;
      public int answers_correct_amount = 0;
    }
    public class Lesson
    {
      public int key = 0;
      public string name = string.Empty;
      public List<Page> pages = new();
      public Lesson()
      {
        key = Guid.NewGuid().GetHashCode();
      }
    }
  }
  public struct ELEMENTS
  {
    public static GameObject EMPTY => Resources.Load<GameObject>("Prefabs/Elements_Page/EMPTY");
    public static GameObject GENERAL_TEXT => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_TEXT");
    public static GameObject GENERAL_WARNING => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_WARNING");
    public static GameObject GENERAL_IMAGE => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_IMAGE");
    public static GameObject GENERAL_LINE => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_LINE");
    public static GameObject GENERAL_CODE_COMMAND => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_CODE_COMMAND");
    public static GameObject GENERAL_CODE_SCRIPT => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_CODE_SCRIPT");
    public static GameObject GENERAL_TTS => Resources.Load<GameObject>("Prefabs/Elements_Page/GENERAL_TTS");
    public static GameObject CHALLENGE_MULTIPLE_CHOICE => Resources.Load<GameObject>("Prefabs/Elements_Page/CHALLENGE_MULTIPLE_CHOICE");
    public static GameObject CHALLENGE_INPUT_NORMAL => Resources.Load<GameObject>("Prefabs/Elements_Page/CHALLENGE_INPUT_NORMAL");
    public static GameObject AI_PROMPT => Resources.Load<GameObject>("Prefabs/Elements_Page/AI_PROMPT");
    public static GameObject SYSTEM_QUIZ_RESULT => Resources.Load<GameObject>("Prefabs/Elements_Page/SYSTEM_QUIZ_RESULT");
    public static GameObject SYSTEM_QUIZ_ANSWERS => Resources.Load<GameObject>("Prefabs/Elements_Page/SYSTEM_QUIZ_ANSWERS");
    public static GameObject SYSTEM_QUIZZES_MENU => Resources.Load<GameObject>("Prefabs/Elements_Page/SYSTEM_QUIZZES_MENU");
  }
}